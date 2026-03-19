using Application.DTOs.UsuarioRecuperacion.Request;
using Domain.Entities;

namespace Application.Interfaces;
public interface IUsuarioRecuperacionCommandUseCase
{
    Task<string> GenerarRecuperacionContrasena(string email);
    Task<VerificacionResultado> VerificarRecuperacion(string codigo, string cedula);
    Task<bool> EstablacerRecuperacionContrasena(UsuarioRecuperacionDto usuarioRecuperacionDto);
}
