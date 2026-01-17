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

            // Rutas públicas que SIEMPRE están permitidas (incluso sin sesión)
            var path = context.Request.Path.Value?.ToLower();
            var rutasPublicas = new[]
            {
                "/",                          // Raíz
                "/usuario/login",             // Login
                "/usuario/logout",            // Logout
                "/registro/crear",            // Registro de estudiante
                "/usuario/recuperacion",      // Recuperación de contraseña
                "/usuario/accesodenegado",    // Página de acceso denegado
                "/home/error",                // Página de error
                "/error",                     // Error genérico
                "/home/privacy",
                "/home/about",
                "/home/contact"
            };

            // Verificar si la ruta actual es pública
            bool esRutaPublica = false;
            if (!string.IsNullOrEmpty(path))
            {
                foreach (var rutaPublica in rutasPublicas)
                {
                    if (path == rutaPublica ||
                        path.StartsWith(rutaPublica + "/") || // Para rutas con parámetros
                        (rutaPublica == "/" && path == "/"))
                    {
                        esRutaPublica = true;
                        break;
                    }
                }
            }

            // Si es ruta pública, permitir acceso
            if (esRutaPublica)
            {
                await _next(context);
                return;
            }

            // Si NO es ruta pública, VERIFICAR SI HAY SESIÓN ACTIVA
            if (!TieneSesionActiva(context))
            {
                // Redirigir al login si no hay sesión
                context.Response.Redirect("/Usuario/Login");
                return;
            }

            // Si tiene sesión, aplicar la lógica original de permisos
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

        private bool TieneSesionActiva(HttpContext context)
        {
            // Verificar autenticación por cookies (Identity)
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                return true;
            }

            // Verificar sesión personalizada
            var session = context.Session;
            var userId = session.GetString("UserId");
            var userName = session.GetString("UserName");
            var isLoggedIn = session.GetString("IsLoggedIn");

            // Si tiene datos de sesión, está autenticado
            return (!string.IsNullOrEmpty(userId) &&
                    !string.IsNullOrEmpty(userName) &&
                    isLoggedIn == "true");
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