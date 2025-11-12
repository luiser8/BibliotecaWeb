namespace Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public int ExtensionId { get; set; }
    public int TipoUsuarioId { get; set; }
    public string Correo { get; set; }
    public string Contrasena { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Extension Extension { get; set; }
    public TipoUsuario TipoUsuario { get; set; }
    public DatosPersonales DatosPersonales { get; set; }
    public DatosAcademicos DatosAcademicos { get; set; }

    public ICollection<Auditoria> Auditorias { get; set; }
    public ICollection<Prestamo> Prestamos { get; set; }
}