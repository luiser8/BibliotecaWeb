namespace Domain.Entities;

public class Autor
{
    public int Id { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public ICollection<Material> Materiales { get; set; }
}