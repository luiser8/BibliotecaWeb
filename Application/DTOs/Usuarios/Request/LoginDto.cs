using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Usuarios.Request;

    public class LoginDto
    {
    private readonly string _emailConfig = "@psm.edu.ve";

        [Required(ErrorMessage = "El correo institucional es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+$",
            ErrorMessage = "Nombre de usuario inválido")]
    public string? Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string? Contrasena { get; set; }

        public bool RememberMe { get; set; }

        // Método de validación personalizada
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!string.IsNullOrEmpty(Correo) && !Correo.EndsWith(_emailConfig, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(new ValidationResult($"Solo se permiten correos del dominio {_emailConfig}",
                    new[] { nameof(Correo) }));
            }

            return results;
        }
    }