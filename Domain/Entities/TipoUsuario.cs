namespace Domain.Entities;

public class TipoUsuario
{
    public int Id { get; set; }
    public int RolId { get; set; }
    public string Tipo { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Rol Rol { get; set; }
    public ICollection<Usuario> Usuarios { get; set; }
}