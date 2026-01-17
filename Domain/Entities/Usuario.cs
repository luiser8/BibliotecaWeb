namespace Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public int? ExtensionId { get; set; }
    public int RolId { get; set; }
    public string? Correo { get; set; }
    public string? Contrasena { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Extension? Extension { get; set; }
    public Rol? Rol { get; set; }
    public DatosPersonales? DatosPersonales { get; set; }
    public DatosAcademicos? DatosAcademicos { get; set; }

    public ICollection<Auditoria>? Auditorias { get; set; }
    public ICollection<Prestamo>? Prestamos { get; set; }

    // Métodos de validación
    public void ValidarId()
    {
        if (Id <= 0)
            throw new ArgumentException("El Id debe ser mayor que 0", nameof(Id));
    }

    public void ValidarRolId()
    {
        if (RolId <= 0)
            throw new ArgumentException("El RolId debe ser mayor que 0", nameof(RolId));
    }

    public void ValidarExtensionId()
    {
        if (ExtensionId.HasValue && ExtensionId <= 0)
            throw new ArgumentException("El ExtensionId debe ser mayor que 0 si está presente", nameof(ExtensionId));
    }

    public void ValidarCorreo()
    {
        if (string.IsNullOrWhiteSpace(Correo))
            throw new ArgumentException("El correo es requerido", nameof(Correo));

        if (!Correo.Contains("@"))
            throw new ArgumentException("El correo no tiene un formato válido", nameof(Correo));
    }
}