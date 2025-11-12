namespace Domain.Entities;

public class TipoMaterial
{
    public int Id { get; set; }
    public string Tipo { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public ICollection<Material> Materiales { get; set; }
    public ICollection<ReglasPrestamo> ReglasPrestamos { get; set; }
}