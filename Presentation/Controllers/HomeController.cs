// HomeController.cs
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class HomeController : Controller
{
    //private readonly AppSettings _appSettings;

    public async Task<IActionResult> Index()
    {
        // El filter CargarPoliticasFilter ya cargó ViewData["PoliticasBasicas"]
        // automáticamente antes de llegar aquí

        // Solo carga otros datos específicos de la página de inicio
        //SetViewDataFromSettings(); // Si tienes este método

        // Obtener datos del usuario para personalizar la vista
        var nombreUsuario = User?.FindFirstValue("NombreCompleto")
                          ?? User?.Identity?.Name
                          ?? "Usuario";

        var rolUsuario = User?.FindFirstValue("Rol")
                       ?? User?.FindFirst(ClaimTypes.Role)?.Value
                       ?? "Usuario";

        // Puedes acceder a las políticas ya cargadas por el filter
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
        //SetViewDataFromSettings();
        return View();
    }

    public IActionResult Contact()
    {
        //SetViewDataFromSettings();

        //ViewData["PuedeContactarSoporte"] = TienePolitica("ContactarSoporte");
        //ViewData["PuedeContactarAdministrador"] = TienePolitica("ContactarAdministrador");

        return View();
    }

    public IActionResult About()
    {
       // SetViewDataFromSettings();
        return View();
    }

    public IActionResult Help()
    {
        //SetViewDataFromSettings();
        return View();
    }

    public IActionResult AccesoDenegado()
    {
        //SetViewDataFromSettings();
        ViewData["Title"] = "Acceso Denegado";

        // Obtener la ruta que intentaba acceder
        var rutaIntentada = HttpContext.Request.Query["ReturnUrl"].ToString();
        if (!string.IsNullOrEmpty(rutaIntentada))
        {
            ViewData["RutaIntentada"] = rutaIntentada;
        }

        // Si es administrador, mostrar más detalles
        if (User.IsInRole("Administrador"))
        {
            ViewData["PoliticasUsuario"] = ViewData["PoliticasUsuario"] as List<PoliticasUsuario> ?? new List<PoliticasUsuario>();
           // ViewData["RutasPermitidas"] = ObtenerRutasPermitidas();
        }

        return View();
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        return View();
    }
}
