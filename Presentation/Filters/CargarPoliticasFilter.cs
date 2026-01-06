// Filters/CargarPoliticasFilter.cs
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Presentation.Filters
{
    public class CargarPoliticasFilter : IAsyncActionFilter
    {
        private readonly IPoliticasUsuarioUseCase _politicasUsuarioUseCase;

        public CargarPoliticasFilter(IPoliticasUsuarioUseCase politicasUsuarioUseCase)
        {
            _politicasUsuarioUseCase = politicasUsuarioUseCase;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // Obtener el controlador actual
            if (context.Controller is Controller controller)
            {
                // Verificar si el usuario está autenticado
                if (controller.User?.Identity?.IsAuthenticated ?? false)
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

                        // Obtener todas las políticas del usuario (Header y Menu)
                        var policies = await _politicasUsuarioUseCase.GetPoliticasAsync(
                            Convert.ToInt16(rolIdClaim),
                            "Header,Menu"
                        );

                        // Preparar las políticas para la vista
                        var politicasBasicas = policies
                            .Select(p => new
                            {
                                PoliticaId = p.PoliticaId,
                                Nombre = p.Nombre,
                                Tipo = p.Tipo,
                                Ruta = p.Ruta,
                                Rol = rolNombre
                            })
                            .ToList();

                        // Pasar a ViewData y ViewBag
                        controller.ViewData["PoliticasBasicas"] = politicasBasicas;
                        controller.ViewBag.PoliticasBasicas = politicasBasicas;
                    }
                    catch (Exception ex)
                    {
                        // Loggear error pero continuar
                        Console.WriteLine($"Error cargando políticas: {ex.Message}");
                        controller.ViewData["PoliticasBasicas"] = new List<dynamic>();
                    }
                }
                else
                {
                    // Usuario no autenticado
                    controller.ViewData["PoliticasBasicas"] = new List<dynamic>();
                    controller.ViewBag.PoliticasBasicas = new List<dynamic>();
                }
            }

            // Continuar con la ejecución normal
            await next();
        }
    }
}