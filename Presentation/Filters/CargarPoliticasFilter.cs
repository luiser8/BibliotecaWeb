// Presentation/Filters/CargarPoliticasFilter.cs (modificado)
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Presentation.Extensions;
using System.Security.Claims;

namespace Presentation.Filters
{
    public class CargarPoliticasFilter : IAsyncActionFilter
    {
        private readonly IPoliticasUsuariosQueryUseCase _politicasUsuarioUseCase;

        public CargarPoliticasFilter(IPoliticasUsuariosQueryUseCase politicasUsuarioUseCase)
        {
            _politicasUsuarioUseCase = politicasUsuarioUseCase;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var session = httpContext.Session;

            // Obtener el controlador actual
            if (context.Controller is Controller controller)
            {
                // Verificar si el usuario está autenticado
                if (controller.User?.Identity?.IsAuthenticated == true)
                {
                    try
                    {
                        var user = controller.User;

                        // Obtener el RolId del usuario
                        var rolIdClaim = user.FindFirst("RolId")?.Value
                                       ?? user.FindFirstValue("RolId")
                                       ?? "0";

                        // Obtener el nombre del rol
                        var rolNombre = user.FindFirst(ClaimTypes.Role)?.Value
                                      ?? user.FindFirstValue("Rol")
                                      ?? "Usuario";

                        var extensionNombre = user.FindFirst("Extension")?.Value
                                             ?? user.FindFirstValue("Extension")
                                             ?? "Extension";

                        // Obtener todas las políticas del usuario (Header y Menu)
                        var policies = await _politicasUsuarioUseCase.GetPoliticasAsync(
                            Convert.ToInt16(rolIdClaim),
                            "Header,Menu,Boton"
                        );

                        // Preparar las políticas para la vista
                        var politicasBasicas = policies
                            .Select(p => new
                            {
                                p.PoliticaId,
                                p.Nombre,
                                p.Tipo,
                                Ruta = p.Ruta?.ToLower(),
                                Rol = rolNombre,
                                //Extension = extensionNombre
                            })
                            .ToList();

                        // Extraer solo las rutas para validación
                        // Reemplazar la asignación de rutasPermitidas para asegurar que sea List<string> y no List<string?>
                        var rutasPermitidas = politicasBasicas
                            .Select(p => p.Ruta?.ToLower())
                            .Where(r => !string.IsNullOrEmpty(r))
                            .Cast<string>() // Asegura que la lista sea de tipo List<string>
                            .ToList();

                        // Guardar en sesión para validación de acceso
                        session.SetStringList("RutasPermitidas", rutasPermitidas);
                        session.SetObject("PoliticasCompletas", politicasBasicas);
                        session.SetString("Extension", extensionNombre);

                        // Pasar a ViewData y ViewBag
                        controller.ViewData["PoliticasBasicas"] = politicasBasicas;
                        controller.ViewBag.PoliticasBasicas = politicasBasicas;
                    }
                    catch (Exception ex)
                    {
                        // Loggear error pero continuar
                        var logger = httpContext.RequestServices.GetService<ILogger<CargarPoliticasFilter>>();
                        logger?.LogError(ex, "Error cargando políticas");
                        controller.ViewData["PoliticasBasicas"] = new List<dynamic>();
                    }
                }
                else
                {
                    // Usuario no autenticado - limpiar sesión
                    session.Remove("RutasPermitidas");
                    session.Remove("PoliticasCompletas");
                    controller.ViewData["PoliticasBasicas"] = new List<dynamic>();
                }
            }

            // Continuar con la ejecución normal
            await next();
        }
    }
}