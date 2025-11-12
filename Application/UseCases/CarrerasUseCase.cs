using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases;

public class CarrerasUseCase(ICarreraRepository carreraRepository)
{
    public async Task<Carrera> ExecuteByIdAsync(int id)
    {
        return await carreraRepository.GetByIdAsync(id);
    }

    public async Task<List<Carrera>> ExecuteAllAsync()
    {
        return await carreraRepository.GetAllAsync();
    }

    public async Task<List<Carrera>> ExecuteAllWithExtensionAsync(int id)
    {
        return await carreraRepository.GetAllWithExtensionAsync(id);
    }
}