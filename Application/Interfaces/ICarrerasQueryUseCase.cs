using Domain.Entities;

namespace Application.Interfaces;

public interface ICarrerasQueryUseCase
{
    public Task<Carrera> ExecuteByIdAsync(int id);

    public Task<List<Carrera>> ExecuteAllAsync();

    public Task<List<Carrera>> ExecuteAllWithExtensionAsync(int id);
}