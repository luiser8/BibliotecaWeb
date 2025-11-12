namespace Domain.Entities;

public class Politica
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public ICollection<RolPolitica> RolPoliticas { get; set; }
}