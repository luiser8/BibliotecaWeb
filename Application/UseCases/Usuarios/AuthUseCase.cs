using System.Security.Claims;
using Application.DTOs.Usuarios.Request;
using Application.DTOs.Usuarios.Responses;
using Application.Interfaces;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Exceptions;

namespace Application.UseCases.Usuarios;

/// <summary>
/// UseCase para manejar la autenticación de usuarios y generación de Claims
/// </summary>
public class AuthUseCase : IAuthUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<OperationResult<AuthResult>> AuthenticateAsync(LoginDto loginDto)
    {
        try
        {
            // Autenticar usuario
            var usuario = await _usuarioRepository.AuthAsync(loginDto.Correo, loginDto.Contrasena);

            if (usuario == null)
            {
                return OperationResult<AuthResult>.Error(
                    "Correo electrónico o contraseña incorrectos. Por favor, verifica tus credenciales.",
                    "AUTH_FAILED");
            }

            // Generar Claims basados en el usuario
            var claims = GenerateClaims(usuario);

            // Calcular fecha de expiración
            var expiresUtc = DateTimeOffset.UtcNow.AddHours(8);

            var authResult = new AuthResult
            {
                UsuarioId = usuario.UsuarioId,
                Correo = usuario.Correo ?? string.Empty,
                NombreCompleto = usuario.Nombres + " " + usuario.Apellidos,
                RolId = usuario.RolId,
                Rol = usuario.Rol ?? string.Empty,
                Claims = claims,
                RememberMe = loginDto.RememberMe,
                ExpiresUtc = expiresUtc
            };

            return OperationResult<AuthResult>.Ok(authResult);
        }
        catch (RecordNotFoundException ex)
        {
            return OperationResult<AuthResult>.Error(
                "Correo electrónico o contraseña incorrectos. Por favor, verifica tus credenciales.",
                ex.ErrorCode);
        }
        catch (RepositoryException ex)
        {
            return OperationResult<AuthResult>.Error(ex.Message, ex.ErrorCode);
        }
        catch (ArgumentException ex)
        {
            return OperationResult<AuthResult>.Error(ex.Message);
        }
        catch (Exception ex)
        {
            return OperationResult<AuthResult>.Error($"Error al autenticar usuario: {ex.Message}");
        }
    }

    /// <summary>
    /// Genera los Claims del usuario basados en su información
    /// </summary>
    private List<Claim> GenerateClaims(AuthUsuario usuario)
    {
        var nombreCompleto = $"{usuario.Nombres} {usuario.Apellidos}".Trim();
        
        var claims = new List<Claim>
        {
            new Claim("UsuarioId", usuario.UsuarioId.ToString()),
            new Claim("Correo", usuario.Correo ?? ""),
            new Claim("Cedula", usuario.Cedula ?? ""),
            new Claim("Contrasena", usuario.Contrasena ?? ""),
            new Claim("Nombres", usuario.Nombres ?? ""),
            new Claim("Apellidos", usuario.Apellidos ?? ""),
            new Claim("NombreCompleto", nombreCompleto), // Agregar nombre completo como claim
            new Claim(ClaimTypes.Name, nombreCompleto), // También como ClaimTypes.Name
            new Claim("RolId", usuario.RolId.ToString()),
            new Claim("Rol", usuario.Rol ?? ""),
            new Claim("ExtensionId", usuario.ExtensionId.ToString()),
            new Claim("Extension", usuario.Extension ?? ""),
            new Claim("CarreraId", usuario.CarreraId.ToString()),
            new Claim("Carrera", usuario.Carrera ?? ""),
            new Claim(ClaimTypes.Email, usuario.Correo ?? "")
        };

        return claims;
    }
}

