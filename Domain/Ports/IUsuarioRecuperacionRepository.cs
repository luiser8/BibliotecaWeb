using Domain.Entities;

namespace Domain.Ports;

public interface IUsuarioRecuperacionRepository
{
    Task<string?> RecoveryAsync(int usuarioId, string codigo);
    Task<UsuarioRecuperacion> VerificarCodigoUsuario(string codigo);
    Task<bool> VencimientoCodigo(string codigo);
}