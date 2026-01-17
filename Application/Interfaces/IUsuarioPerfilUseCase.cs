using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUsuarioPerfilUseCase
    {
        Task<bool> CambiarContrasenaAsync(int UsuarioId, string newPassword);
    }
}
