using Application.DTOs.Usuarios.Request;
using Application.DTOs.Usuarios.Responses;

namespace Application.Interfaces;

public interface IAuthQueryUseCase
{
    /// <summary>
    /// Autentica un usuario y genera los Claims necesarios
    /// </summary>
    Task<OperationResult<AuthResult>> AuthenticateAsync(LoginDto loginDto);
}

