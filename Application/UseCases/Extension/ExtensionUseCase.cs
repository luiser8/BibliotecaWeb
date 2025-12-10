using Application.DTOs.Extension.Responses;
using Application.Interfaces;
using Domain.Ports;

namespace Application.UseCases.Extension;

public class ExtensionQueryUseCase(IExtensionRepository extensionRepository) : IExtensionQueryUseCase
{
    public async Task<Domain.Entities.Extension> ExecuteByIdAsync(int id)
    {
        return await extensionRepository.GetByIdAsync(id);
    }

    public async Task<List<Domain.Entities.Extension>> ExecuteAllAsync()
    {
        return await extensionRepository.GetAllAsync();
    }

    public async Task<List<ExtensionCarrerasResponse>> ExecuteAllWithExtensionAsync()
    {
        var request = await extensionRepository.GetAllWithCarrerasAsync();
    
        return request.Select(item =>
            {
                if (item == null) return null;

                return new ExtensionCarrerasResponse
                {
                    ExtensionId = item.ExtensionId,
                    Nombre = item.Nombre ?? string.Empty,
                    Ciudad = item.Ciudad ?? string.Empty,
                    Estado = item.Estado ?? string.Empty,
                    Direccion = item.Direccion ?? string.Empty,
                    Activo = item.Activo,
                    Extension = item.Extension != null ? new ExtensionResponse
                    {
                        Id = item.Extension.Id,
                        Nombre = item.Extension.Nombre ?? string.Empty,
                        Descripcion = item.Extension.Descripcion ?? string.Empty,
                        Direccion = item.Extension.Direccion ?? string.Empty,
                        Estado = item.Extension.Estado ?? string.Empty,
                        Ciudad = item.Extension.Ciudad ?? string.Empty,
                        Defecto = item.Extension.Defecto != 0,
                        Activo = item.Extension.Activo == 0 ? false : true ,
                    } : null,
                    Carreras = item.Carreras?.Select(c => new CarreraExtensionResponse
                    {
                        Id = c?.Id ?? 0,
                        Carrera = c?.Nombre ?? string.Empty
                    }).ToList() ?? new List<CarreraExtensionResponse>()
                };
            })
            .Where(item => item != null)
            .ToList();
    }
}
