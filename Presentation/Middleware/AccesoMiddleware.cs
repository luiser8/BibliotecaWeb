// Presentation/Middleware/AccesoMiddleware.cs
using System.Text.Json;

namespace Presentation.Middleware
{
    public class AccesoMiddleware
    {
        private readonly RequestDelegate _next;

        public AccesoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Solo validar rutas de MVC
            if (context.Request.Path.StartsWithSegments("/api") ||
                context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/_framework") ||
                context.Request.Path.Value?.Contains(".") == true) // Archivos estáticos
            {
                await _next(context);
                return;
            }

            // Rutas que siempre están permitidas (inicio, login, logout, error)
            var path = context.Request.Path.Value?.ToLower();
            var rutasPermitidas = new[]
            {
                "/",
                "/usuario/login",
                "/usuario/logout",
                "/usuario/accesodenegado",
                "/home/index",
                "/home/error",
                "home/privacy",
                "home/about",
                "home/contact",
                "/error"
            };

            if (rutasPermitidas.Any(r => path?.StartsWith(r) == true))
            {
                await _next(context);
                return;
            }

            // Verificar autenticación
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var session = context.Session;
                var rutasUsuarioJson = session.GetString("RutasPermitidas");

                if (!string.IsNullOrEmpty(rutasUsuarioJson))
                {
                    var rutasUsuario = JsonSerializer.Deserialize<List<string>>(rutasUsuarioJson);

                    if (rutasUsuario != null && rutasUsuario.Any())
                    {
                        // Extraer ruta MVC (Controller/Action)
                        var rutaMvc = GetMvcRoute(path);

                        if (!string.IsNullOrEmpty(rutaMvc) && !rutasUsuario.Contains(rutaMvc.ToLower()))
                        {
                            // Obtener logger del servicio
                            var logger = context.RequestServices.GetService<ILogger<AccesoMiddleware>>();
                            logger?.LogWarning(
                                "Middleware: Acceso denegado para {Usuario} a {Ruta}",
                                context.User.Identity.Name,
                                path);

                            context.Response.Redirect("/Usuario/AccesoDenegado");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }

        private string GetMvcRoute(string path)
        {
            if (string.IsNullOrEmpty(path) || path == "/")
                return "Home/Index";

            // Remover "/" inicial
            path = path.TrimStart('/');

            // Si tiene query string, removerlo
            var queryIndex = path.IndexOf('?');
            if (queryIndex > 0)
                path = path.Substring(0, queryIndex);

            // Convertir "/Controller/Action" a "Controller/Action"
            var parts = path.Split('/');
            if (parts.Length >= 2)
            {
                return $"{parts[0]}/{parts[1]}";
            }
            else if (parts.Length == 1)
            {
                return $"{parts[0]}/Index";
            }

            return path;
        }
    }
}