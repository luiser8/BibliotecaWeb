namespace Domain.Entities;

public class RolPolitica
{
    public int Id { get; set; }
    public int RolId { get; set; }
    public int PoliticaId { get; set; }
    
    public Rol Rol { get; set; }
    public Politica Politica { get; set; }
}