namespace Application.DTOs.Usuarios.Request;

public class DatosAcademicosDto
{
    public int UsuarioId { get; set; }
    public int CarreraId { get; set; }
    public string? TipoIngreso { get; set; }
}