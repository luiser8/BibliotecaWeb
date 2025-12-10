namespace Domain.Entities;

public class Prestamo
{
    public int Id { get; set; }
    public int ReglaPrestamoId { get; set; }
    public int MaterialId { get; set; }
    public int? UsuarioId { get; set; }
    public string? Estatus { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public ReglasPrestamo? ReglasPrestamo { get; set; }
    public Material? Material { get; set; }
    public Usuario? Usuario { get; set; }
    public ICollection<Morosidad>? Morosidades { get; set; }
}