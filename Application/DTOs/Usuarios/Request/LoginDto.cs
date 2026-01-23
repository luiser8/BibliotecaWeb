using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Usuarios.Request;

public class LoginDto
{
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    [Display(Name = "Nombre de usuario")]
    public string? Usuario { get; set; }

    // Este campo se llenará automáticamente con JavaScript
    public string? Correo { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string? Contrasena { get; set; }

    [Display(Name = "Recordar sesión")]
    public bool RememberMe { get; set; }
}