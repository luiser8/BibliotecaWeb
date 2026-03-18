namespace Application.DTOs.UsuarioRecuperacion.Request
{
    public class UsuarioRecuperacionDto
    {
        public int? UsuarioId { get; set; }
        public string? Cedula { get; set; }
        public string? Codigo { get; set; }
        public string? NuevaContrasena { get; set; }
    }
}
