using Domain.Commands;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Exceptions;
using Infrastructure.Handlers;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Data;

namespace Infrastructure.Repositories
{
    public class UsuarioPerfilRepository : IUsuarioPerfilRepository
    {
        private readonly IDataTableExecute _dbCon;
        private DataTable? _dt;
        private readonly Hashtable _params;
        private readonly IPasswordHasher? _passwordHasher;
        private readonly ILogger<UsuarioRepository>? _logger;

        public UsuarioPerfilRepository(
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

        public async Task<bool> CambiarContrasenaAsync(Usuario usuario)
        {
            return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
            {
                usuario.ValidarId();

                _params.Clear();
                _params.Add("@Id", usuario.Id);
                _params.Add("@NewContrasena", _passwordHasher?.HashPasswordAsync(usuario.Contrasena) ?? usuario.Contrasena);

                _dt = await _dbCon.ExecuteAsync(nameof(EUsuarioCommand.SPUsuarioUpdatePasswordCommand), _params);

                if (_dt.Rows.Count == 0)
                    return false;

                // Verificar si hay error
                if (_dt.Columns.Contains("ErrorMessage") && _dt.Rows[0]["ErrorMessage"] != DBNull.Value)
                {
                    string? errorMessage = _dt.Rows[0]["ErrorMessage"].ToString();
                    throw new RepositoryException("UPDATE_FAILED", errorMessage);
                }

                return true;

            }, "ActualizarContrasenaUsuario");
        }
    }
}
