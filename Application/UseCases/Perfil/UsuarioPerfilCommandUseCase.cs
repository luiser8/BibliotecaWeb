using Application.Interfaces;
using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases.Perfil
{
    public class UsuarioPerfilCommandUseCase : IUsuarioPerfilCommandUseCase
    {
        private readonly IUsuarioPerfilRepository _usuarioPerfilRepository;

        public UsuarioPerfilCommandUseCase(IUsuarioPerfilRepository usuarioPerfilRepository)
        {
            _usuarioPerfilRepository = usuarioPerfilRepository;
        }

        public async Task<bool> CambiarContrasenaAsync(int UsuarioId, string newPassword)
        {
            return await _usuarioPerfilRepository.CambiarContrasenaAsync(new Usuario { Id = UsuarioId, Contrasena = newPassword });
        }
    }
}
