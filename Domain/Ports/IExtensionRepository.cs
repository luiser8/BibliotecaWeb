using Domain.Entities;

namespace Domain.Ports;

public interface IExtensionRepository
{
    Task<Extension> GetByIdAsync(int id);
    Task<List<Extension>> GetAllAsync();
    Task<List<Extension>> GetAllWithCarrerasAsync();
}