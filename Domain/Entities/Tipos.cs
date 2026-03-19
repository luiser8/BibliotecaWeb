namespace Domain.Entities;

public class Tipos
{
    public int Id { get; set; }
    public string? Tipo { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreado { get; set; }
    public ICollection<Materiales> Materiales { get; set; }
    public ICollection<ReglasPrestamo> ReglasPrestamos { get; set; }
}