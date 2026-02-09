using Application.DTOs.UsuarioRecuperacion.Request;
using Application.Interfaces;
using Domain.Ports;

namespace Application.UseCases.UsuarioRecuperacion
{
    public class UsuarioRecuperacionCommandUseCase : IUsuarioRecuperacionCommandUseCase
    {
        private readonly ICodigoRecuperacion _codigoRecuperacion;

        public UsuarioRecuperacionCommandUseCase(ICodigoRecuperacion codigoRecuperacion)
        {
            _codigoRecuperacion = codigoRecuperacion;
        }

        public string GenerarRecuperacionContrasena()
        {
            return _codigoRecuperacion.StringCodigoAsync();
        }
    }
}
