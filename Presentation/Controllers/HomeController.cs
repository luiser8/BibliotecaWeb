// HomeController.cs
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class HomeController : Controller
{
    //private readonly AppSettings _appSettings;

    public async Task<IActionResult> Index()
    {
        // Obtener datos del usuario para personalizar la vista
        var nombreUsuario = User?.FindFirstValue("NombreCompleto")
                          ?? User?.Identity?.Name
                          ?? "Usuario";

        var rolUsuario = User?.FindFirstValue("Rol")
                       ?? User?.FindFirst(ClaimTypes.Role)?.Value
                       ?? "Usuario";

        // Puedes acceder a las pol�ticas ya cargadas por el filter
        var politicasBasicas = ViewData["PoliticasBasicas"] as IEnumerable<dynamic>
                              ?? Enumerable.Empty<dynamic>();

        ViewData["NombreUsuario"] = nombreUsuario;
        ViewData["RolUsuario"] = rolUsuario;
        ViewData["PoliticasCount"] = politicasBasicas.Count();

        // Preparar datos para el dashboard (si es necesario)
        var dashboard = new DashBoard
        {
            TotalBooks = 12500,
            AvailableBooks = 8500,
            ActiveLoans = 3200,
            PendingReturns = 150
        };

        return View(dashboard);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Help()
    {
        return View();
    }

    public IActionResult AccesoDenegado()
    {
        ViewData["Title"] = "Acceso Denegado";

        // Obtener la ruta que intentaba acceder
        var rutaIntentada = HttpContext.Request.Query["ReturnUrl"].ToString();
        if (!string.IsNullOrEmpty(rutaIntentada))
        {
            ViewData["RutaIntentada"] = rutaIntentada;
        }

        // Si es administrador, mostrar m�s detalles
        if (User.IsInRole("Administrador"))
        {
            ViewData["PoliticasUsuario"] = ViewData["PoliticasUsuario"] as List<PoliticasUsuario> ?? new List<PoliticasUsuario>();
        }

        return View();
    }

    [HttpGet]
    public IActionResult Error()
    {
        var statusCode = HttpContext.Response.StatusCode;
        ViewBag.StatusCode = statusCode;

        ViewBag.ErrorMessage = statusCode == 404
            ? "La página que buscas no existe o fue escrita incorrectamente."
            : "Ocurrió un error inesperado.";

        return View();
    }
}
