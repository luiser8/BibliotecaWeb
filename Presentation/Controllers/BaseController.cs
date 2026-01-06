// Controllers/BaseController.cs
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly AppSettings _appSettings;

        // Propiedad para acceder a las políticas desde controladores hijos
        protected List<PoliticasUsuario> PoliticasUsuario =>
            ViewData["PoliticasUsuario"] as List<PoliticasUsuario> ?? new List<PoliticasUsuario>();

        // Constructor que solo recibe AppSettings
        protected BaseController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

        /// <summary>
        /// Se ejecuta antes de cada acción
        /// </summary>
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            await CargarPoliticasUsuario();
            await base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// Carga las políticas del usuario actual
        /// </summary>
        private async Task CargarPoliticasUsuario()
        {
            try
            {
                if (User?.Identity?.IsAuthenticated != true)
                {
                    ViewData["PoliticasUsuario"] = new List<PoliticasUsuario>();
                    return;
                }

                // Obtener el servicio de políticas dinámicamente
                var politicasService = HttpContext.RequestServices
                    .GetService<IPoliticasUsuarioUseCase>();

                if (politicasService == null)
                {
                    ViewData["PoliticasUsuario"] = new List<PoliticasUsuario>();
                    return;
                }

                var userId = GetCurrentUserId();
                var userRole = User?.FindFirst(ClaimTypes.Role)?.Value;

                if (userId.HasValue && !string.IsNullOrEmpty(userRole))
                {
                    var politicas = await politicasService.GetPoliticasAsync(4, "Header");

                    ViewData["PoliticasUsuario"] = politicas;
                    ViewBag.PoliticasUsuario = politicas;
                }
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error cargando políticas: {ex.Message}");
                ViewData["PoliticasUsuario"] = new List<PoliticasUsuario>();
            }
        }

        /// <summary>
        /// Verifica si el usuario tiene acceso a una ruta específica
        /// </summary>
        protected bool TieneAccesoRuta(string ruta)
        {
            if (string.IsNullOrEmpty(ruta))
                return true;

            return PoliticasUsuario.Any(p =>
                !string.IsNullOrEmpty(p.Ruta) &&
                p.Ruta.Equals(ruta, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si el usuario tiene una política por nombre
        /// </summary>
        protected bool TienePolitica(string nombrePolitica)
        {
            return PoliticasUsuario.Any(p =>
                !string.IsNullOrEmpty(p.Nombre) &&
                p.Nombre.Equals(nombrePolitica, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si el usuario tiene alguna de las políticas especificadas
        /// </summary>
        protected bool TieneAlgunaPolitica(params string[] nombresPoliticas)
        {
            return PoliticasUsuario.Any(p =>
                !string.IsNullOrEmpty(p.Nombre) &&
                nombresPoliticas.Contains(p.Nombre, StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Filtra una lista de items de navegación según las políticas del usuario
        /// </summary>
        protected List<NavbarItem> FiltrarNavbarPorPoliticas(List<NavbarItem> navbarItems)
        {
            if (navbarItems == null || !navbarItems.Any())
                return new List<NavbarItem>();

            return navbarItems.Where(item =>
            {
                // Si no requiere política específica, mostrar siempre
                if (string.IsNullOrEmpty(item.RequiredPolicy))
                    return true;

                // Verificar si el usuario tiene la política requerida
                return TienePolitica(item.RequiredPolicy) ||
                       TieneAccesoRuta($"/{item.Controller}/{item.Action}");
            }).ToList();
        }

        /// <summary>
        /// Obtiene las rutas permitidas para el usuario actual
        /// </summary>
        protected List<string> ObtenerRutasPermitidas()
        {
            return PoliticasUsuario
                .Where(p => !string.IsNullOrEmpty(p.Ruta))
                .Select(p => p.Ruta)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Establece los ViewData desde la configuración de la aplicación
        /// </summary>
        protected void SetViewDataFromSettings()
        {
            if (_appSettings == null)
            {
                SetDefaultViewData();
                return;
            }

            // Configuración general
            ViewData["SiteTitle"] = _appSettings.SiteTitle;
            ViewData["LibraryName"] = _appSettings.LibraryName;
            ViewData["LibraryLogo"] = _appSettings.LibraryLogo;
            ViewData["LibraryLogoAlt"] = _appSettings.LibraryLogoAlt;
            ViewData["LibrarySlogan"] = _appSettings.LibrarySlogan;
            ViewData["FooterText"] = _appSettings.FooterText;
            ViewData["SupportEmail"] = _appSettings.SupportEmail;
            ViewData["SupportPhone"] = _appSettings.SupportPhone;
            ViewData["LibraryHours"] = _appSettings.LibraryHours;

            // Items de navegación filtrados por políticas
            ViewData["NavbarItems"] = FiltrarNavbarPorPoliticas(_appSettings.NavbarItems);
            ViewData["AdminNavbarItems"] = FiltrarNavbarPorPoliticas(_appSettings.AdminNavbarItems);
            ViewData["QuickLinks"] = _appSettings.QuickLinks;
        }

        /// <summary>
        /// Establece valores por defecto para ViewData
        /// </summary>
        private void SetDefaultViewData()
        {
            ViewData["SiteTitle"] = "Sistema de Biblioteca";
            ViewData["LibraryName"] = "Biblioteca Universitaria";
            // ... resto de valores por defecto

            // Items de navegación por defecto filtrados
            var defaultNavbar = new List<NavbarItem>
            {
                new NavbarItem { Text = "Inicio", Controller = "Home", Action = "Index", Icon = "fas fa-home" },
                new NavbarItem { Text = "Catálogo", Controller = "Catalogo", Action = "Index", Icon = "fas fa-book" },
                new NavbarItem { Text = "Mis Préstamos", Controller = "Prestamos", Action = "Index", Icon = "fas fa-book-reader", RequiredPolicy = "VerPrestamos" }
            };

            ViewData["NavbarItems"] = FiltrarNavbarPorPoliticas(defaultNavbar);
        }

        /// <summary>
        /// Obtiene el ID del usuario actual desde los claims
        /// </summary>
        protected int? GetCurrentUserId()
        {
            var userIdClaim = User?.FindFirst("UserId") ?? User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        /// <summary>
        /// Redirige al usuario si no tiene acceso a la ruta actual
        /// </summary>
        protected IActionResult RedirigirSiNoTieneAcceso(string rutaRequerida = null)
        {
            var rutaActual = rutaRequerida ?? $"{ControllerContext.RouteData.Values["controller"]}/{ControllerContext.RouteData.Values["action"]}";

            if (!TieneAccesoRuta(rutaActual))
            {
                TempData["ErrorMessage"] = "No tiene permisos para acceder a esta sección.";
                return RedirectToAction("Index", "Home");
            }

            return null;
        }

    }
}