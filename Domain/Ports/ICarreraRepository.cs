using Domain.Entities;

namespace Domain.Ports;

public interface ICarreraRepository
{
    Task<Carrera> GetByIdAsync(int id);
    Task<List<Carrera>> GetAllAsync();
    Task<List<Carrera>> GetAllWithExtensionAsync(int id);
}