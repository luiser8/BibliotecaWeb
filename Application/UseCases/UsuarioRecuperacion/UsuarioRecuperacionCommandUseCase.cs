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
        await _emailPort.SendEmailAsync(saveCodigo ?? string.Empty, "Recuperación de contraseña", $"Tu código de recuperación es: {codigo}");

        return codigo;
    }

    public async Task<bool> EstablacerRecuperacionContrasena(UsuarioRecuperacionDto usuarioRecuperacionDto)
    {
        if (string.IsNullOrEmpty(usuarioRecuperacionDto.Codigo) || string.IsNullOrEmpty(usuarioRecuperacionDto.Cedula))
            throw new NotImplementedException();

        var resultado = await VerificarRecuperacion(usuarioRecuperacionDto.Codigo, usuarioRecuperacionDto.Cedula);

        if (!resultado.EsValido)
            throw new Exception(resultado.MensajeError);

        var save = await _usuarioPerfilRepository.CambiarContrasenaAsync(new Usuario
        {
            Id = resultado.UsuarioId ?? 0,
            Contrasena = usuarioRecuperacionDto.NuevaContrasena ?? string.Empty
        });

        if (!save)
            throw new Exception("Error no se pudo cambiar la contraseña");

        return resultado.EsValido;
    }

    public async Task<VerificacionResultado> VerificarRecuperacion(string codigo, string cedula)
    {
        if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(cedula))
            return new VerificacionResultado(false, null, "Código o cédula no proporcionados", "");

        var verificarUsuarioCodigo = await _usuarioRecuperacionRepository.VerificarCodigoUsuario(codigo);

        if (verificarUsuarioCodigo.Id == null || verificarUsuarioCodigo.Codigo == null)
            return new VerificacionResultado(false, null, "Error código de seguridad no existe", "");

        if (!verificarUsuarioCodigo.Activo)
            return new VerificacionResultado(false, null, "Error código de seguridad vencido o no existe", verificarUsuarioCodigo.Codigo);

        if (verificarUsuarioCodigo.Cedula != cedula)
            return new VerificacionResultado(false, null, "Error código de seguridad no pertenece a este usuario", verificarUsuarioCodigo.Codigo);

        if (DateTime.Now - verificarUsuarioCodigo.FechaCreado > TimeSpan.FromHours(24))
            return new VerificacionResultado(false, null, "Error código de seguridad vencido por mas de 24 horas", verificarUsuarioCodigo.Codigo);

        return new VerificacionResultado(true, verificarUsuarioCodigo.UsuarioId, "Código válido", verificarUsuarioCodigo.Codigo);
    }
}
