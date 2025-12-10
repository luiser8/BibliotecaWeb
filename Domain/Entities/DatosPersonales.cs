namespace Domain.Entities;

public class DatosPersonales
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string? Cedula { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? FechaNacimiento { get; set; }
    public string? Sexo { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Usuario Usuario { get; set; }
}