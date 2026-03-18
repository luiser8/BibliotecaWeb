using Application.DTOs.UsuarioRecuperacion.Request;
using Application.Interfaces;
using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases.UsuarioRecuperacion;

public class UsuarioRecuperacionCommandUseCase : IUsuarioRecuperacionCommandUseCase
{
    private readonly IDatosPersonalesRepository _datosPersonalesRepository;
    private readonly ICodigoRecuperacion _codigoRecuperacion;
    private readonly IEmailPort _emailPort;
    private readonly IUsuarioRecuperacionRepository _usuarioRecuperacionRepository;
    private readonly IUsuarioPerfilRepository _usuarioPerfilRepository;

    public UsuarioRecuperacionCommandUseCase(
        IDatosPersonalesRepository datosPersonalesRepository,
        ICodigoRecuperacion codigoRecuperacion,
        IEmailPort emailPort,
        IUsuarioRecuperacionRepository usuarioRecuperacionRepository,
        IUsuarioPerfilRepository usuarioPerfilRepository)
    {
        _datosPersonalesRepository = datosPersonalesRepository;
        _codigoRecuperacion = codigoRecuperacion;
        _emailPort = emailPort;
        _usuarioRecuperacionRepository = usuarioRecuperacionRepository;
        _usuarioPerfilRepository = usuarioPerfilRepository;
    }

    public async Task<string> GenerarRecuperacionContrasena(string cedula)
    {
        var datosPersonales = await _datosPersonalesRepository.ExistsByCedula(cedula) ?? throw new Exception("El email no existe.");
        var codigo = await _codigoRecuperacion.StringCodigoAsync() ?? throw new Exception("No se pudo generar el código de recuperación.");

        // Guardar el código con el id del usuario en la base de datos
        var saveCodigo = await _usuarioRecuperacionRepository.RecoveryAsync(datosPersonales.UsuarioId, codigo);

        // Enviar el código al email del usuario
        //await _emailPort.SendEmailAsync(saveCodigo ?? string.Empty, "Recuperación de contraseña", $"Tu código de recuperación es: {codigo}");

        return codigo;
    }

    public async Task<bool> EstablacerRecuperacionContrasena(UsuarioRecuperacionDto usuarioRecuperacionDto)
    {
        if(string.IsNullOrEmpty(usuarioRecuperacionDto.Codigo)) throw new NotImplementedException();
        var verificarUsuarioCodigo = await _usuarioRecuperacionRepository.VerificarCodigoUsuario(usuarioRecuperacionDto.Codigo);
        if(!verificarUsuarioCodigo.Activo)
            throw new Exception("Error código de seguridad vencido");
        var save= await _usuarioPerfilRepository.CambiarContrasenaAsync(new Usuario { Id = verificarUsuarioCodigo.UsuarioId ?? 0, Contrasena = usuarioRecuperacionDto.NuevaContrasena ?? string.Empty });
        if (!save)
            throw new Exception("Error no se pude cambiar la contraseña");
        return await _usuarioRecuperacionRepository.VencimientoCodigo(usuarioRecuperacionDto.Codigo);
    }
}
