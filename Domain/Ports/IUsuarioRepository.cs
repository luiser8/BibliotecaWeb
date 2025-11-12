using Domain.Entities;

namespace Domain.Ports;

public interface IUsuarioRepository
{
    Task<int> AddAsync(Usuario usuario);
    Task<Usuario> AuthAsync(string correo, string contrasena);
}