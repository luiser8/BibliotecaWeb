namespace Domain.Entities;

public class Carrera
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public ICollection<ExtensionCarrera> ExtensionCarreras { get; set; }
    public ICollection<DatosAcademicos> DatosAcademicos { get; set; }
}