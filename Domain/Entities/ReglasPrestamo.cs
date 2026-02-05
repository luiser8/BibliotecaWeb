namespace Domain.Entities;

public class ReglasPrestamo
{
    public int Id { get; set; }
    public int TipoId { get; set; }
    public int DiasLimites { get; set; }
    public int HorasLimites { get; set; }
    public int CantidadLimites { get; set; }
    public decimal? Mora { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Tipos Tipos { get; set; }
    public Prestamo Prestamo { get; set; }
}