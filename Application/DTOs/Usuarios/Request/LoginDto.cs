using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Usuarios.Request;

public class LoginDto
{
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contrasena { get; set; }

        public bool RememberMe { get; set; }
    }
