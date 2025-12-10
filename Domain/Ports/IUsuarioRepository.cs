using Domain.Entities;

namespace Domain.Ports;

public interface IUsuarioRepository
{
    Task<int> AddAsync(Usuario usuario);
    Task<AuthUsuario> AuthAsync(string correo, string contrasena);
}