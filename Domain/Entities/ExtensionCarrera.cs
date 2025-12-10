namespace Domain.Entities;

public class ExtensionCarrera
{
    public int Id { get; set; }
    public int ExtensionId { get; set; }
    public int CarreraId { get; set; }
    public bool Activo { get; set; }
    public string? Nombre { get; set; }
    public string? Ciudad { get; set; }
    public string? Estado { get; set; }
    public string? Direccion { get; set; }
    public DateTime FechaCreado { get; set; }
    public Extension? Extension { get; set; }
    //public Carrera? Carrera { get; set; }
    public List<Carrera>? Carreras { get; set; } = [];
}
