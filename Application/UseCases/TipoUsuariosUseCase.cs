using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases;

public class TipoUsuariosUseCase(ITipoUsuariosRepository tipoUsuariosRepository)
{
    public async Task<TipoUsuario> ExecuteByIdAsync(int id)
    {
        return await tipoUsuariosRepository.GetByIdAsync(id);
    }

    public async Task<List<TipoUsuario>> ExecuteAllAsync()
    {
        return await tipoUsuariosRepository.GetAllWithRolesAsync();
    }
}