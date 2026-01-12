using Application.DTOs.Usuarios.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class UsuarioController : Controller
{
    private readonly IAuthUseCase _authUseCase;

    public UsuarioController(IAuthUseCase authUseCase)
    {
        _authUseCase = authUseCase;
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
        HttpContext.Session.Clear();
        TempData["SuccessMessage"] = "Sesión cerrada exitosamente";
        return RedirectToAction("Login", "Usuario");
    }
}
