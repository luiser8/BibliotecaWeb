using Domain.Entities;

namespace Domain.Ports;

public interface IRolesRepository
{
    Task<Rol> GetAsync(int id);
    Task<List<Rol>> GetAllAsync();
}