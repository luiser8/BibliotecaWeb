namespace Domain.Entities;

public class Materiales
{
    public int Id { get; set; }
    public int ExtensionId { get; set; }
    public int CategoriaId { get; set; }
    public int EditorialId { get; set; }
    public int AutorId { get; set; }
    public string? Cota { get; set; }
    public string? Titulo { get; set; }
    public string? Edicion { get; set; }
    public string? Recurso { get; set; }
    public string? EjemplaresActuales { get; set; }
    public string? EjemplaresTotales { get; set; }
    public string? Estatus { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
    
    public Categorias? CategoriaMaterial { get; set; }
    public Extension? Extension { get; set; }
    public Autor? Autor { get; set; }
    public Editoriales? Editorial { get; set; }
    public ICollection<Prestamo>? Prestamos { get; set; }
}