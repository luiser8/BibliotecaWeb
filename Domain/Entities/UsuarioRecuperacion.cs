namespace Domain.Entities;

public class UsuarioRecuperacion
{
  public int? Id { get; set; }
  public int? UsuarioId { get; set; }
  public string? Cedula { get; set; }
  public string? Codigo { get; set; }
  public bool Activo { get; set; }
  public DateTime? FechaCreado { get; set; }
}

public record VerificacionResultado(bool EsValido, int? UsuarioId, string MensajeError, string codigo);