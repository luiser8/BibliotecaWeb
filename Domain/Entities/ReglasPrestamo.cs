namespace Domain.Entities;

public class ReglasPrestamo
{
    public int Id { get; set; }
    public int TipoMaterialId { get; set; }
    public int DiasLimites { get; set; }
    public int HorasLimites { get; set; }
    public int CantidadLimites { get; set; }
    public decimal? Mora { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public TipoMaterial TipoMaterial { get; set; }
}