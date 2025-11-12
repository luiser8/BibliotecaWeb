using Domain.Entities;

namespace Domain.Ports;

public interface ITipoUsuariosRepository
{
    Task<TipoUsuario> GetByIdAsync(int id);
    Task<List<TipoUsuario>> GetAllWithRolesAsync();
}