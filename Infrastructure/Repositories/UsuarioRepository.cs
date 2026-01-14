using System.Data;
using System.Collections;
using Domain.Commands;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Handlers;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;
    
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDataTableExecute _dbCon;
        private DataTable? _dt;
        private readonly Hashtable _params;
        private readonly IPasswordHasher? _passwordHasher;
        private readonly ILogger<UsuarioRepository>? _logger;
        
        public UsuarioRepository(
            IDataTableExecute dataTableExecute, 
            IPasswordHasher? passwordHasher,
            ILogger<UsuarioRepository>? logger = null)
        {
            _dt = new DataTable();
            _dbCon = dataTableExecute ?? throw new ArgumentNullException(nameof(dataTableExecute));
            _params = [];
            _passwordHasher = passwordHasher;
            _logger = logger;
        }
        
        public async Task<int> AddAsync(Usuario usuario)
        {
            return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
            {
                // Validaciones
                ValidateEntity(usuario, nameof(usuario));
                
                if (string.IsNullOrWhiteSpace(usuario.Correo))
                    throw new ArgumentException("El correo es requerido", nameof(usuario.Correo));
                
                if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                    throw new ArgumentException("La contraseña es requerida", nameof(usuario.Contrasena));
                
                // Verificar si el correo ya existe
                /*var existe = await ExistsByEmailAsync(usuario.Correo);
                if (existe)
                    throw new DuplicateRecordException("Correo", usuario.Correo);
                */
                
                _params.Clear();
                _params.Add("@ExtensionId", usuario.ExtensionId);
                _params.Add("@RolId", usuario.RolId);
                _params.Add("@Correo", usuario.Correo);
                _params.Add("@Contrasena", _passwordHasher?.HashPasswordAsync(usuario.Contrasena) ?? usuario.Contrasena);

                _dt = await _dbCon.ExecuteAsync(nameof(EUsuarioCommand.SPUsuarioAddCommand), _params);
                
                if (_dt.Rows.Count == 0)
                    throw new RepositoryException("ADD_FAILED", "No se pudo crear el usuario");
                
                // Verificar si el procedimiento almacenado devuelve un error
                if (_dt.Columns.Contains("ErrorMessage") && _dt.Rows[0]["ErrorMessage"] != DBNull.Value)
                {
                    string errorMessage = _dt.Rows[0]["ErrorMessage"].ToString();
                    int errorCode = _dt.Columns.Contains("ErrorCode") ? 
                        Convert.ToInt32(_dt.Rows[0]["ErrorCode"]) : 0;
                    
                    // Manejar errores específicos del SP
                    if (errorCode == 2601 || errorCode == 2627 || 
                        errorMessage.Contains("correo", StringComparison.OrdinalIgnoreCase) ||
                        errorMessage.Contains("duplicado", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new DuplicateRecordException("Correo", usuario.Correo);
                    }
                    
                    throw new RepositoryException($"SP_ERROR_{errorCode}", errorMessage);
                }
                
                return Convert.ToInt32(_dt.Rows[0]["Id"]);
                
            }, "AgregarUsuario");
        }
        
        public async Task<AuthUsuario> AuthAsync(string correo, string contrasena)
        {
            return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(correo))
                    throw new ArgumentException("El correo es requerido", nameof(correo));
        
                if (string.IsNullOrWhiteSpace(contrasena))
                    throw new ArgumentException("La contraseña es requerida", nameof(contrasena));
        
                // 1. Obtener el usuario por correo
                _params.Clear();
                _params.Add("@Correo", correo);
        
                _dt = await _dbCon.ExecuteAsync(nameof(EUsuarioCommand.SPUsuarioAuthCommand), _params);
        
                if (_dt.Rows.Count == 0)
                    throw new RecordNotFoundException("Usuario", $"Correo: {correo}");
        
                var usuario = MapDataRowToAuthUsuario(_dt.Rows[0]);
                var contrasenaAlmacenada = _dt.Rows[0]["Contrasena"]?.ToString();
        
                if (string.IsNullOrEmpty(contrasenaAlmacenada))
                    throw new UnauthorizedAccessException("Credenciales inválidas");
        
                // 2. Verificar contraseña en código C#
                bool esValida = false;
        
                if (_passwordHasher == null)
                {
                    // Sin hasher, comparación directa (NO RECOMENDADO)
                    esValida = contrasena == contrasenaAlmacenada;
                }
                else
                {
                    esValida = _passwordHasher.VerifyPasswordAsync(contrasena, contrasenaAlmacenada);
                }
        
                return !esValida ? throw new UnauthorizedAccessException("Credenciales inválidas") : usuario;
            }, "AutenticarUsuario");
        }
        
        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
            {
                ValidateId(id, "id de usuario");
                
                _params.Clear();
                _params.Add("@Id", id);
                
                _dt = await _dbCon.ExecuteAsync(nameof(EUsuarioCommand.SPUsuarioGetByIdCommand), _params);
                
                if (_dt.Rows.Count == 0)
                    throw new RecordNotFoundException("Usuario", id);
                
                return MapDataRowToUsuario(_dt.Rows[0]);
                
            }, "ObtenerUsuarioPorId");
        }
        
        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
            {
                ValidateEntity(usuario, nameof(usuario));
                ValidateId(usuario.Id, "id de usuario");
                
                // Verificar si el usuario existe
                var usuarioExistente = await GetByIdAsync(usuario.Id);
                if (usuarioExistente == null)
                    throw new RecordNotFoundException("Usuario", usuario.Id);
                
                // Si cambió el correo, verificar que no exista otro con ese correo
                /*if (!string.Equals(usuarioExistente.Correo, usuario.Correo, StringComparison.OrdinalIgnoreCase))
                {
                    var existe = await ExistsByEmailAsync(usuario.Correo);
                    if (existe)
                        throw new DuplicateRecordException("Correo", usuario.Correo);
                }*/
                
                _params.Clear();
                _params.Add("@Id", usuario.Id);
                _params.Add("@ExtensionId", usuario.ExtensionId);
                _params.Add("@RolId", usuario.RolId);
                _params.Add("@Correo", usuario.Correo);
                
                // Solo actualizar contraseña si se proporcionó una nueva
                if (!string.IsNullOrWhiteSpace(usuario.Contrasena))
                {
                    _params.Add("@Contrasena", _passwordHasher?.HashPasswordAsync(usuario.Contrasena) ?? usuario.Contrasena);
                }
                
                _dt = await _dbCon.ExecuteAsync(nameof(EUsuarioCommand.SPUsuarioUpdateCommand), _params);
                
                if (_dt.Rows.Count == 0)
                    return false;
                
                // Verificar si hay error
                if (_dt.Columns.Contains("ErrorMessage") && _dt.Rows[0]["ErrorMessage"] != DBNull.Value)
                {
                    string? errorMessage = _dt.Rows[0]["ErrorMessage"].ToString();
                    throw new RepositoryException("UPDATE_FAILED", errorMessage);
                }
                
                return true;
                
            }, "ActualizarUsuario");
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
            {
                ValidateId(id, "id de usuario");
                
                // Verificar si el usuario existe
                await GetByIdAsync(id); // Esto lanzará RecordNotFoundException si no existe
                
                _params.Clear();
                _params.Add("@Id", id);
                
                _dt = await _dbCon.ExecuteAsync(nameof(EUsuarioCommand.SPUsuarioDeleteCommand), _params);
                
                if (_dt.Rows.Count == 0)
                    return false;
                
                // Verificar si hay error (ej: violación FK)
                if (_dt.Columns.Contains("ErrorMessage") && _dt.Rows[0]["ErrorMessage"] != DBNull.Value)
                {
                    string errorMessage = _dt.Rows[0]["ErrorMessage"].ToString();
                    
                    if (errorMessage.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
                        errorMessage.Contains("relacionado", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ForeignKeyViolationException("FK_Usuario", "tablas relacionadas");
                    }
                    
                    throw new RepositoryException("DELETE_FAILED", errorMessage);
                }
                
                return true;
                
            }, "EliminarUsuario");
        }
        
        // Métodos de ayuda privados
    private AuthUsuario MapDataRowToAuthUsuario(DataRow row)
    {
        try
        {
            var authUsuario = new AuthUsuario
            {
                UsuarioId = row["UsuarioId"] != DBNull.Value ? Convert.ToInt32(row["UsuarioId"]) : 0,
                Cedula = row["Cedula"]?.ToString() ?? string.Empty,
                Correo = row["Correo"]?.ToString() ?? string.Empty,
                Nombres = row["Nombres"]?.ToString() ?? string.Empty,
                Apellidos = row["Apellidos"]?.ToString() ?? string.Empty,
                RolId = row["RolId"] != DBNull.Value ? Convert.ToInt32(row["RolId"]) : 0,
                Rol = row["Rol"]?.ToString() ?? string.Empty,
                ExtensionId = row["ExtensionId"] != DBNull.Value ? Convert.ToInt32(row["ExtensionId"]) : 0,
                Extension = row["Extension"]?.ToString() ?? string.Empty,
                CarreraId = row["CarreraId"] != DBNull.Value ? Convert.ToInt32(row["CarreraId"]) : 0,
                Carrera = row["Carrera"]?.ToString() ?? string.Empty
            };

            return authUsuario;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error al mapear DataRow a AuthUsuario");
            throw new RepositoryException("MAPPING_ERROR", 
                "Error al convertir los datos del auth usuario: " + ex.Message);
        }
    }
        
        private Usuario MapDataRowToUsuario(DataRow row)
        {
            try
            {
                return new Usuario
                {
                    Id = row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : 0,
                    ExtensionId = row["ExtensionId"] != DBNull.Value ? Convert.ToInt32(row["ExtensionId"]) : null,
                    RolId = row["RolId"] != DBNull.Value ? Convert.ToInt32(row["RolId"]) : 0,
                    Correo = row["Correo"]?.ToString(),
                    Contrasena = row["Contrasena"]?.ToString(),
                    // Agrega más propiedades según tu modelo
                    FechaCreado = row["FechaCreacion"] != DBNull.Value ? 
                        Convert.ToDateTime(row["FechaCreacion"]) : DateTime.MinValue,
                    Activo = row["Activo"] == DBNull.Value || Convert.ToBoolean(row["Activo"])
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al mapear DataRow a Usuario");
                throw new RepositoryException("MAPPING_ERROR", 
                    "Error al convertir los datos del usuario: " + ex.Message);
            }
        }
        
        private void ValidateEntity(Usuario usuario, string paramName)
        {
            if (usuario == null)
                throw new ArgumentNullException(paramName, $"{paramName} no puede ser nulo");
        }
        
        private void ValidateId(int id, string idName)
        {
            if (id <= 0)
                throw new ArgumentException($"{idName} debe ser mayor que 0", idName);
        }
        
        // Método para obtener todos los usuarios (opcional)
        public async Task<IEnumerable<Usuario>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
            {
                _params.Clear();
                _params.Add("@PageNumber", page);
                _params.Add("@PageSize", pageSize);
                
                _dt = await _dbCon.ExecuteAsync(nameof(EUsuarioCommand.SPUsuarioGetAllCommand), _params);
                
                var usuarios = new List<Usuario>();
                
                foreach (DataRow row in _dt.Rows)
                {
                    usuarios.Add(MapDataRowToUsuario(row));
                }
                
                return usuarios;
                
            }, "ObtenerTodosUsuarios");
        }
    }