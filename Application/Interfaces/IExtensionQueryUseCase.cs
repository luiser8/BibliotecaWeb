using Application.DTOs.Extension.Responses;

namespace Application.Interfaces;

public interface IExtensionQueryUseCase
{
    public Task<Domain.Entities.Extension> ExecuteByIdAsync(int id);

    public Task<List<Domain.Entities.Extension>> ExecuteAllAsync();

    public Task<List<ExtensionCarrerasResponse>> ExecuteAllWithExtensionAsync();
}