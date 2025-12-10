namespace Application.DTOs.Usuarios.Request;

public class DatosPersonalesDto
{
    public int UsuarioId { get; set; }
    public string? Cedula { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? FechaNacimiento { get; set; }
    public string? Sexo { get; set; }
}