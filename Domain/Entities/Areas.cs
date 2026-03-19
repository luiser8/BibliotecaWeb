namespace Domain.Entities;

public class Areas
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreado { get; set; }
    public ICollection<Materiales>? Materiales { get; set; }
}