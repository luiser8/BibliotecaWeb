namespace Domain.Entities;

public class Auditoria
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Tabla { get; set; }
    public string Accion { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Usuario Usuario { get; set; }
}