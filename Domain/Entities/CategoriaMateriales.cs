namespace Domain.Entities;

public class CategoriaMateriales
{
    public int Id { get; set; }
    public string? Categoria { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreado { get; set; }
    public ICollection<Material> Materiales { get; set; }
    public ICollection<ReglasPrestamo> ReglasPrestamos { get; set; }
}