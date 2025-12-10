namespace Application.DTOs.Usuarios.Request;

public class UsuarioCreateDto
{
    public int ExtensionId { get; set; }
    public int RolId { get; set; }
    public string? Correo { get; set; }
    public string? Contrasena { get; set; }
    public DatosPersonalesDto DatosPersonales { get; set; }
    public DatosAcademicosDto DatosAcademicos { get; set; }
}