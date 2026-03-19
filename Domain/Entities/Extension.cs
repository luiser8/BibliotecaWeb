namespace Domain.Entities;

public class Extension
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Estado { get; set; }
    public string Ciudad { get; set; }
    public string Direccion { get; set; }
    public int Defecto { get; set; }
    public int Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public ICollection<Usuario>? Usuarios { get; set; }
    public ICollection<ExtensionCarrera>? ExtensionCarreras { get; set; }
}
