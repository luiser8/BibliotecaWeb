using Application.Interfaces;
using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases.Carreras;

public class CarrerasQueryUseCase(ICarreraRepository carreraRepository)  :ICarrerasQueryUseCase
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