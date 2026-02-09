using Application.DTOs.UsuarioRecuperacion.Request;
using Application.DTOs.UsuarioRecuperacion.Responses;

namespace Application.Interfaces;
public interface IUsuarioRecuperacionCommandUseCase
{
    /// <summary>
    /// Autentica un usuario y genera los Claims necesarios
    /// </summary>
    string GenerarRecuperacionContrasena();
}
