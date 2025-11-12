namespace Domain.Entities;

public class DatosAcademicos
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int CarreraId { get; set; }
    public string TipoIngreso { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Usuario Usuario { get; set; }
    public Carrera Carrera { get; set; }
}