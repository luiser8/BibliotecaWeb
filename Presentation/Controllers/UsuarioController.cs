using Application.Domain.Configuration;
using Application.DTOs.Usuarios.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Middleware;

namespace Presentation.Controllers;

public class UsuarioController : Controller
{
    private readonly IAuthQueryUseCase _authUseCase;
    private readonly EmailConfig _emailConfig;

    public UsuarioController(IAuthQueryUseCase authUseCase, IOptions<EmailConfig> emailConfig)
    {
        _authUseCase = authUseCase;
        _emailConfig = emailConfig.Value;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        return View();
    }

    // GET: /Account/Login
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["Domain"] = _emailConfig.EmailServerAccept;
        ViewData["EmailDomain"] = _emailConfig.EmailServerAccept.Replace("@", "@");
        ViewData["Arroba"] = "@";

        // Si ya está autenticado, redirigir al home
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model, string? returnUrl = null)
    {
        // Si recibimos solo el nombre de usuario, construir el correo completo
        if (!string.IsNullOrEmpty(model.Correo))
        {
            // Extraer solo el nombre de usuario (limpia @gmail.com, @hotmail.com, etc.)
            var nombreUsuario = model.Correo.ExtraerNombreUsuario(_emailConfig.EmailServerAccept);

            // Construir correo institucional
            model.Correo = nombreUsuario.ConstruirCorreoInstitucional(_emailConfig.EmailServerAccept);
        } else
        {
            ModelState.AddModelError("Correo", $"Solo se permiten correos del dominio {_emailConfig.EmailServerAccept}");
            ViewData["Domain"] = _emailConfig.EmailServerAccept;
            ViewData["EmailDomain"] = _emailConfig.EmailServerAccept.Replace("@", "@@");
            return View(model);
        }

        var result = await _authUseCase.AuthenticateAsync(model);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.ErrorMessage ?? "Error al iniciar sesión");
            TempData["ErrorMessage"] = result.ErrorMessage ?? "Error al iniciar sesión";
            return View(model);
        }

        // Crear ClaimsIdentity y AuthenticationProperties desde el resultado
        var authResult = result.Data!;
        var claimsIdentity = new System.Security.Claims.ClaimsIdentity(
            authResult.Claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = authResult.RememberMe,
            ExpiresUtc = authResult.ExpiresUtc
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new System.Security.Claims.ClaimsPrincipal(claimsIdentity),
            authProperties);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    // GET: /Account/Logout
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        TempData["SuccessMessage"] = "Sesión cerrada exitosamente";
        return RedirectToAction("Login", "Usuario");
    }
}
