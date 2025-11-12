namespace Domain.Entities;

public class Material
{
    public int Id { get; set; }
    public int TipoMaterialId { get; set; }
    public int AutorId { get; set; }
    public string Cota { get; set; }
    public string Titulo { get; set; }
    public string Edicion { get; set; }
    public string Editorial { get; set; }
    public string Anio { get; set; }
    public string Estatus { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public TipoMaterial TipoMaterial { get; set; }
    public Autor Autor { get; set; }
    public ICollection<Prestamo> Prestamos { get; set; }
}