// HomeController.cs
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Controllers;
using System.Security.Claims;

public class HomeController : BaseController
{
    private readonly AppSettings _appSettings;

    public HomeController(IOptions<AppSettings> appSettings) : base(appSettings)
    {
        _appSettings = appSettings?.Value;
    }

    public async Task<IActionResult> Index()
    {
        // El filter CargarPoliticasFilter ya cargó ViewData["PoliticasBasicas"]
        // automáticamente antes de llegar aquí

        // Solo carga otros datos específicos de la página de inicio
        SetViewDataFromSettings(); // Si tienes este método

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
        SetViewDataFromSettings();
        return View();
    }

    public IActionResult Contact()
    {
        SetViewDataFromSettings();

        ViewData["PuedeContactarSoporte"] = TienePolitica("ContactarSoporte");
        ViewData["PuedeContactarAdministrador"] = TienePolitica("ContactarAdministrador");

        return View();
    }

    public IActionResult About()
    {
        SetViewDataFromSettings();
        return View();
    }

    public IActionResult Help()
    {
        SetViewDataFromSettings();
        return View();
    }

    public IActionResult AccesoDenegado()
    {
        SetViewDataFromSettings();
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
            ViewData["RutasPermitidas"] = ObtenerRutasPermitidas();
        }

        return View();
    }

    [HttpGet]
    public IActionResult PoliticasUsuario()
    {
        SetViewDataFromSettings();

        // Solo usuarios autenticados pueden ver sus políticas
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction("Login", "Usuario");
        }

        var politicas = ViewData["PoliticasUsuario"] as List<PoliticasUsuario> ?? new List<PoliticasUsuario>();

        // Crear modelo con las políticas del usuario
        var modelo = new PoliticasUsuarioViewModel
        {
            Politicas = politicas,
            TotalPoliticas = politicas.Count,
            RutasPermitidas = ObtenerRutasPermitidas(),
            Usuario = User?.Identity?.Name ?? "Usuario",
            Rol = User?.FindFirstValue("Rol") ?? User?.FindFirst(ClaimTypes.Role)?.Value ?? "Sin rol"
        };

        return View(modelo);
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        SetViewDataFromSettings();

        // Verificar acceso al dashboard
        if (!TienePolitica("VerDashboard") && !User.IsInRole("Administrador"))
        {
            TempData["ErrorMessage"] = "No tiene permiso para acceder al dashboard";
            return RedirectToAction("Index");
        }

        // Cargar datos del dashboard según permisos
        var dashboardData = new DashboardCompletoViewModel();

        // Estadísticas básicas
        dashboardData.EstadisticasGenerales = new EstadisticasGenerales
        {
            TotalLibros = 12500,
            LibrosDisponibles = 8500,
            PrestamosActivos = 3200,
            UsuariosRegistrados = 2450
        };

        // Datos sensibles solo para usuarios con permisos específicos
        if (TienePolitica("VerEstadisticasFinancieras"))
        {
            dashboardData.EstadisticasFinancieras = new EstadisticasFinancieras
            {
                IngresosMensuales = 125000,
                GastosMensuales = 85000,
                MultasCobradas = 15000,
                PresupuestoAnual = 1500000
            };
        }

        if (TienePolitica("VerEstadisticasUsuarios"))
        {
            dashboardData.EstadisticasUsuarios = new EstadisticasUsuarios
            {
                NuevosUsuariosMes = 150,
                UsuariosActivos = 2100,
                UsuariosInactivos = 350,
                PrestamosPorUsuario = 1.3m
            };
        }

        // Widgets disponibles basados en políticas
        dashboardData.WidgetsDisponibles = new List<string>
        {
            "resumen", "prestamos", "libros"
        };

        if (TienePolitica("VerWidgetUsuarios"))
            dashboardData.WidgetsDisponibles.Add("usuarios");

        if (TienePolitica("VerWidgetFinanzas"))
            dashboardData.WidgetsDisponibles.Add("finanzas");

        if (TienePolitica("VerWidgetReportes"))
            dashboardData.WidgetsDisponibles.Add("reportes");

        return View(dashboardData);
    }
}

// Modelos para las vistas
public class PoliticasUsuarioViewModel
{
    public List<PoliticasUsuario> Politicas { get; set; }
    public int TotalPoliticas { get; set; }
    public List<string> RutasPermitidas { get; set; }
    public string Usuario { get; set; }
    public string Rol { get; set; }
}

public class DashboardCompletoViewModel
{
    public EstadisticasGenerales EstadisticasGenerales { get; set; }
    public EstadisticasFinancieras EstadisticasFinancieras { get; set; }
    public EstadisticasUsuarios EstadisticasUsuarios { get; set; }
    public List<string> WidgetsDisponibles { get; set; }
}

public class EstadisticasGenerales
{
    public int TotalLibros { get; set; }
    public int LibrosDisponibles { get; set; }
    public int PrestamosActivos { get; set; }
    public int UsuariosRegistrados { get; set; }
}

public class EstadisticasFinancieras
{
    public decimal IngresosMensuales { get; set; }
    public decimal GastosMensuales { get; set; }
    public decimal MultasCobradas { get; set; }
    public decimal PresupuestoAnual { get; set; }
}

public class EstadisticasUsuarios
{
    public int NuevosUsuariosMes { get; set; }
    public int UsuariosActivos { get; set; }
    public int UsuariosInactivos { get; set; }
    public decimal PrestamosPorUsuario { get; set; }
}