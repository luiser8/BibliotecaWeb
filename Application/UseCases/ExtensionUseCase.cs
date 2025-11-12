using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases;

public class ExtensionUseCase(IExtensionRepository extensionRepository)
{
    public async Task<Extension> ExecuteByIdAsync(int id)
    {
        return await extensionRepository.GetByIdAsync(id);
    }

    public async Task<List<Extension>> ExecuteAllAsync()
    {
        return await extensionRepository.GetAllAsync();
    }

    public async Task<List<Extension>> ExecuteAllWithExtensionAsync()
    {
        return await extensionRepository.GetAllWithCarrerasAsync();
    }
}