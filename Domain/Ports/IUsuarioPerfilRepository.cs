using Domain.Entities;

namespace Domain.Ports
{
    public interface IUsuarioPerfilRepository
    {
        Task<bool> CambiarContrasenaAsync(Usuario usuario);
    }
}
