namespace Domain.Entities;

public class Editoriales
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Anio { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
}