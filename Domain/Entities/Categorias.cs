namespace Domain.Entities;

public class Categorias
{
    public int Id { get; set; }
    public string? Categoria { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreado { get; set; }
    public ICollection<Materiales> Materiales { get; set; }
    public ICollection<ReglasPrestamo> ReglasPrestamos { get; set; }
}