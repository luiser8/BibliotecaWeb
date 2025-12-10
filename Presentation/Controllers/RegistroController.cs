using Application.DTOs.Usuarios.Request;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Presentation.Services;

namespace Presentation.Controllers;

public class RegistroController : Controller
{
    private readonly IExtensionQueryUseCase _extensionUseCase;
    private readonly IUsuarioCommandUseCase _usuarioCommandUseCase;
    private readonly ExceptionHandlerService _exceptionHandler;

    public RegistroController(
        IExtensionQueryUseCase extensionUseCase, 
        IUsuarioCommandUseCase usuarioCommandUseCase,
        ExceptionHandlerService exceptionHandler)
    {
        _extensionUseCase = extensionUseCase;
        _usuarioCommandUseCase = usuarioCommandUseCase;
        _exceptionHandler = exceptionHandler;
    }

    // GET: Mostrar formulario de creación
    public async Task<IActionResult> Crear()
    {
        var response = await _extensionUseCase.ExecuteAllWithExtensionAsync();
        return View(response);
    }

    // POST: Procesar el formulario
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(UsuarioCreateDto usuario)
    {
        if (!ModelState.IsValid)
        {
            var response = await _extensionUseCase.ExecuteAllWithExtensionAsync();
            return View(response);
        }

        var result = await _usuarioCommandUseCase.ExecuteInsertAsync(usuario);

        if (result.Success)
        {
            TempData["SuccessMessage"] = "Usuario creado exitosamente";
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
            ModelState.AddModelError("", result.ErrorMessage ?? "Error al crear usuario");
        }

        TempData["ErrorMessage"] = result.ErrorMessage ?? "Error al crear usuario";
        
        var viewResponse = await _extensionUseCase.ExecuteAllWithExtensionAsync();
        return View(viewResponse);
    }
}