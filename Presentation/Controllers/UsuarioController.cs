using Application.DTOs.Usuarios.Request;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Presentation.Controllers;

public class UsuarioController : BaseController
{
    private readonly AppSettings? _appSettings;
    private readonly IAuthUseCase _authUseCase;

    public UsuarioController(IOptions<AppSettings> appSettings, IAuthUseCase authUseCase) : base(appSettings)
    {
        _appSettings = appSettings?.Value;
        _authUseCase = authUseCase;
    }

    // GET: /Account/Login
    public IActionResult Login(string? returnUrl = null)
    {
        SetViewDataFromSettings();
        ViewData["ReturnUrl"] = returnUrl;

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
        SetViewDataFromSettings();

        if (!ModelState.IsValid)
        {
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
        TempData["SuccessMessage"] = "Sesión cerrada exitosamente";
        return RedirectToAction("Login", "Usuario");
    }

    // GET: /Account/AccessDenied
    public IActionResult AccessDenied()
    {
        return View();
    }
}
