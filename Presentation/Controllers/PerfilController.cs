using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

public class PerfilController : Controller
{
    private readonly IUsuarioPerfilCommandUseCase _usuarioPerfilUseCase;

    public PerfilController(IUsuarioPerfilCommandUseCase usuarioPerfilUseCase)
    {
        _usuarioPerfilUseCase = usuarioPerfilUseCase;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewData["UsuarioId"] = User?.FindFirstValue("UsuarioId") ?? "";
        ViewData["NombresUsuario"] = User?.FindFirstValue("NombreCompleto") ?? "";
        ViewData["SexoUsuario"] = User?.FindFirstValue("Sexo") ?? "";
        ViewData["TipoIngresoUsuario"] = User?.FindFirstValue("TipoIngreso") ?? "";
        ViewData["CedulaUsuario"] = User?.FindFirstValue("Cedula") ?? "";
        ViewData["CorreoUsuario"] = User?.FindFirstValue("Correo") ?? "";
        ViewData["RolUsuario"] = User?.FindFirstValue("Rol") ?? "";
        ViewData["ExtensionUsuario"] = User?.FindFirstValue("Extension") ?? "";
        ViewData["CarreraUsuario"] = User?.FindFirstValue("Carrera") ?? "";

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CambiarContrasena(string confirmarContrasena)
    {
        try
        {
            var usuarioId = Convert.ToInt32(User?.FindFirstValue("UsuarioId"));
            var save = await _usuarioPerfilUseCase.CambiarContrasenaAsync(usuarioId, confirmarContrasena);

            if (!save)
            {
                TempData["SuccessMessage"] = "Contraseña cambiada exitosamente, sera cerrada la sesión.";
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Usuario");
            }
            else
            {
                TempData["ErrorMessage"] = "Error al cambiar la contraseña";
                return RedirectToAction("Index", "Perfil");
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error: {ex.Message}";
            return RedirectToAction("Index", "Perfil");
        }
    }
}