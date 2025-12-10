using System.Security.Claims;

namespace Application.DTOs.Usuarios.Responses;

/// <summary>
/// Resultado de autenticación que incluye los Claims del usuario
/// </summary>
public class AuthResult
{
    public int UsuarioId { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public int TipoUsuarioId { get; set; }
    public List<Claim> Claims { get; set; } = [];
    public bool RememberMe { get; set; }
    public DateTimeOffset ExpiresUtc { get; set; }
}

