using Application.DTOs.Extension.Responses;
using Domain.Entities;

namespace Application.Interfaces;

public interface IExtensionQueryUseCase
{
    public Task<Extension> ExecuteByIdAsync(int id);

    public Task<List<Extension>> ExecuteAllAsync();

    public Task<List<ExtensionCarrerasResponse>> ExecuteAllWithExtensionAsync();
}