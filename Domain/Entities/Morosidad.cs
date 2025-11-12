namespace Domain.Entities;

public class Morosidad
{
    public int Id { get; set; }
    public int PrestamoId { get; set; }
    public decimal Monto { get; set; }
    public string Estatus { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Prestamo Prestamo { get; set; }
}