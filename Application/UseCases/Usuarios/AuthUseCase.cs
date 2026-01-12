using System.Security.Claims;
using System.Text.Json;
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

            // Determinar el rol basado en TipoUsuarioId
            var rol = DetermineRole(usuario.RolId);
            
            // Calcular fecha de expiración
            var expiresUtc = DateTimeOffset.UtcNow.AddHours(8);

            var authResult = new AuthResult
            {
                UsuarioId = usuario.UsuarioId,
                Correo = usuario.Correo ?? string.Empty,
                NombreCompleto = usuario.Nombres + " " + usuario.Apellidos,
                RolId = usuario.RolId,
                Rol = rol,
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
            new Claim("Cedula", usuario.Cedula ?? ""),
            new Claim("Nombres", usuario.Nombres ?? ""),
            new Claim("Apellidos", usuario.Apellidos ?? ""),
            new Claim("NombreCompleto", nombreCompleto), // Agregar nombre completo como claim
            new Claim(ClaimTypes.Name, nombreCompleto), // También como ClaimTypes.Name
            new Claim("RolId", usuario.RolId.ToString()),
            new Claim("Rol", usuario.Rol ?? ""),
            new Claim("Extension", usuario.Extension ?? ""),
            new Claim("Carrera", usuario.Carrera ?? ""),
            new Claim(ClaimTypes.Email, usuario.Correo ?? "")
        };

        return claims;
    }

    /// <summary>
    /// Determina el rol del usuario basado en su TipoUsuarioId
    /// </summary>
    private string DetermineRole(int rolId)
    {
        return rolId switch
        {
            1 => "Administrador",
            2 => "Director",
            3 => "Bibliotecario",
            4 => "Estudiante",
            5 => "Profesor",
            _ => throw new ArgumentOutOfRangeException(nameof(rolId), rolId, null)
        };
    }
}

