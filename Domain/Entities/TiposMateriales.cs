namespace Domain.Entities;

public class TiposMateriales
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public int TipoId { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Materiales? Materiales { get; set; }
}