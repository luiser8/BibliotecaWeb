using Domain.Commands;
using Domain.Entities;
using Domain.Ports;
using Domain.Exceptions;
using Infrastructure.Handlers;
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

        public UsuarioPerfilRepository(
        IDataTableExecute dataTableExecute,
        IPasswordHasher? passwordHasher)
        {
            _dt = new DataTable();
            _dbCon = dataTableExecute ?? throw new ArgumentNullException(nameof(dataTableExecute));
            _params = [];
            _passwordHasher = passwordHasher;
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

                return _dt.Rows.Count == 0 ? throw new RepositoryException("UPDATE_FAILED", (Exception)_dt.Rows[0]["ErrorMessage"]) : true;

            }, "ActualizarContrasenaUsuario");
        }
    }
}
