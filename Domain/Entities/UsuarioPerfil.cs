namespace Domain.Entities
{
    public class UsuarioPerfil
    {
        public int UsuarioId { get; set; }
        public string? Cedula { get; set; }
        public string? Correo { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public int RolId { get; set; }
        public string? Rol { get; set; }
        public int ExtensionId { get; set; }
        public string? Extension { get; set; }
        public int CarreraId { get; set; }
        public string? Carrera { get; set; }
    }
}
