// Presentation/Filters/ValidarAccesoFilter.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Presentation.Extensions;

namespace Presentation.Filters
{
    public class ValidarAccesoFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var session = httpContext.Session;

            // Si el usuario está autenticado
            if (httpContext.User?.Identity?.IsAuthenticated == true)
            {
                var controllerName = context.RouteData.Values["controller"]?.ToString();
                var actionName = context.RouteData.Values["action"]?.ToString();
                var rutaActual = $"{controllerName}/{actionName}".ToLower();

                // Normalizar ruta
                rutaActual = rutaActual?.TrimStart('/').ToLower();

                // Obtener rutas permitidas desde la sesión
                var rutasPermitidas = session.GetStringList("RutasPermitidas");

                // Si no hay rutas en sesión, cargarlas desde ViewData/ViewBag
                if (!rutasPermitidas.Any())
                {
                    if (context.Controller is Controller controller)
                    {
                        var politicas = controller.ViewBag.PoliticasBasicas as List<dynamic>;
                        if (politicas != null)
                        {
                            rutasPermitidas = politicas
                                .Select(p => (string)p.Ruta?.ToString().ToLower())
                                .Where(r => !string.IsNullOrEmpty(r))
                                .ToList();

                            // Guardar en sesión para próximas solicitudes
                            session.SetStringList("RutasPermitidas", rutasPermitidas);
                        }
                    }
                }

                // Lista de rutas públicas que no requieren validación
                var rutasPublicas = new List<string>
                {
                    "usuario/login",
                    "usuario/logout",
                    "usuario/accesodenegado",
                    "usuario/error",
                    "home/index",
                    "home/error"
                };

                // Verificar acceso
                bool tieneAcceso = rutasPublicas.Contains(rutaActual) ||
                                  rutasPermitidas.Contains(rutaActual);

                if (!tieneAcceso)
                {
                    // Registrar intento de acceso no autorizado
                    var logger = context.HttpContext.RequestServices.GetService<ILogger<ValidarAccesoFilter>>();
                    logger?.LogWarning(
                        "Acceso denegado para {Usuario} a {Ruta}",
                        httpContext.User.Identity?.Name,
                        rutaActual);

                    context.Result = new RedirectToActionResult("AccesoDenegado", "Usuario", null);
                    return;
                }
            }

            await next();
        }
    }
}