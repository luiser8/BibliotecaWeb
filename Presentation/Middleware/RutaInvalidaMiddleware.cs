// Middleware/RutaInvalidaMiddleware.cs
namespace Presentation.Middleware;
    public class RutaInvalidaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RutaInvalidaMiddleware> _logger;

        public RutaInvalidaMiddleware(RequestDelegate next,
            ILogger<RutaInvalidaMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Logear el error si lo deseas
                _logger.LogError(ex, "Error en la solicitud");

                // Redirigir a Home/Error
                context.Response.Redirect("/Home/Error");
                return;
            }

            // Si llegamos aquí y es un 404, redirigir a Home/Error
            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                var rutaOriginal = context.Request.Path;
                _logger.LogWarning($"Ruta no encontrada: {rutaOriginal}");

                context.Response.Redirect("/Home/Error");
            }
        }
    }