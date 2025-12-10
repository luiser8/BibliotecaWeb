namespace Application.DTOs.Extension.Responses;

public class ExtensionCarrerasResponse
{
    public int? ExtensionId { get; set; }
    public string? Nombre { get; set; }
    public string? Ciudad { get; set; }
    public string? Estado { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; }
    public ExtensionResponse? Extension { get; set; }
    public List<CarreraExtensionResponse>? Carreras { get; set; } = [];
}

public class ExtensionResponse
{
    public int? Id { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public string? Direccion { get; set; }
    public string? Estado { get; set; }
    public string? Ciudad { get; set; }
    public bool? Defecto { get; set; }
    public bool? Activo { get; set; }
    public DateTime FechaCreado { get; set; }
}

public class CarreraExtensionResponse
{
    public int? Id { get; set; }
    public string? Carrera { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreado { get; set; }
}