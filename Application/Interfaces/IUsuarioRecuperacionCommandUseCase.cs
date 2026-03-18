using Application.DTOs.UsuarioRecuperacion.Request;

namespace Application.Interfaces;
public interface IUsuarioRecuperacionCommandUseCase
{
    /// <summary>
    /// Autentica un usuario y genera los Claims necesarios
    /// </summary>
    Task<string> GenerarRecuperacionContrasena(string email);

    Task<bool> EstablacerRecuperacionContrasena(UsuarioRecuperacionDto usuarioRecuperacionDto);
}
