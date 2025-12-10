using Application.DTOs.Usuarios.Request;
using Application.DTOs.Usuarios.Responses;

namespace Application.Interfaces;

public interface IUsuarioCommandUseCase
{
    public Task<OperationResult<int>> ExecuteInsertAsync(UsuarioCreateDto usuario);
}