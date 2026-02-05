using Application.Domain.Configuration;
using Application.DTOs.Usuarios.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Middleware;
using Presentation.Services;

namespace Presentation.Controllers;

public class RegistroController : Controller
{
    private readonly IExtensionQueryUseCase _extensionUseCase;
    private readonly IUsuarioCommandUseCase _usuarioCommandUseCase;
    private readonly ExceptionHandlerService _exceptionHandler;
    private readonly EmailConfig _emailConfig;

    public RegistroController(
        IExtensionQueryUseCase extensionUseCase, 
        IUsuarioCommandUseCase usuarioCommandUseCase,
        ExceptionHandlerService exceptionHandler,
        IOptions<EmailConfig> emailConfig)
    {
        _extensionUseCase = extensionUseCase;
        _usuarioCommandUseCase = usuarioCommandUseCase;
        _exceptionHandler = exceptionHandler;
        _emailConfig = emailConfig.Value;
    }

    // GET: Mostrar formulario de creación de estudiante
    public async Task<IActionResult> Crear()
    {
        var extensionesAsync = await _extensionUseCase.ExecuteAllWithExtensionAsync();
       
        ViewData["Domain"] = _emailConfig.EmailServerAccept;
        ViewData["EmailDomain"] = _emailConfig.EmailServerAccept.Replace("@", "@");
        ViewData["Arroba"] = "@";

        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(extensionesAsync);
    }

    // POST: Procesar el formulario de estudiante
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(UsuarioCreateDto usuario)
    {
        if (!ModelState.IsValid)
        {
            var response = await _extensionUseCase.ExecuteAllWithExtensionAsync();
            return View(response);
        }

        // Si recibimos solo el nombre de usuario, construir el correo completo
        if (!string.IsNullOrEmpty(usuario.Correo))
        {
            // Extraer solo el nombre de usuario (limpia @gmail.com, @hotmail.com, etc.)
            var nombreUsuario = usuario.Correo.ExtraerNombreUsuario(_emailConfig.EmailServerAccept);

            // Construir correo institucional
            usuario.Correo = nombreUsuario.ConstruirCorreoInstitucional(_emailConfig.EmailServerAccept);
        } else
        {
            ModelState.AddModelError("Correo", $"Solo se permiten correos del dominio {_emailConfig.EmailServerAccept}");
            ViewData["Domain"] = _emailConfig.EmailServerAccept;
            ViewData["EmailDomain"] = _emailConfig.EmailServerAccept.Replace("@", "@@");
            return View(usuario);
        }
        
        var result = await _usuarioCommandUseCase.ExecuteInsertAsync(usuario);

        if (result.Success)
        {
            TempData["SuccessMessage"] = "Registro creado exitosamente";
            return RedirectToAction("Login", "Usuario");
        }

        // Manejar errores de validación
        if (result.ValidationErrors != null)
        {
            foreach (var error in result.ValidationErrors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }
        else
        {
            ModelState.AddModelError("", result.ErrorMessage ?? "Error al intentar registrarse");
        }

        TempData["ErrorMessage"] = result.ErrorMessage ?? "Error al intentar registrarse";
        
        var viewResponse = await _extensionUseCase.ExecuteAllWithExtensionAsync();
        return View(viewResponse);
    }
}