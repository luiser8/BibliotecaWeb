using Application.DTOs.UsuarioRecuperacion.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [AllowAnonymous]
    public class RecuperacionController : Controller
    {
        private readonly IUsuarioRecuperacionCommandUseCase _usuarioRecuperacionCommandUseCase;

        public RecuperacionController(IUsuarioRecuperacionCommandUseCase usuarioRecuperacionCommandUseCase)
        {
            _usuarioRecuperacionCommandUseCase = usuarioRecuperacionCommandUseCase;
        }

        // GET: /Recuperacion
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Recuperacion/Generar
        [HttpPost]
        public async Task<IActionResult> Generar(string cedula)
        {
            if (string.IsNullOrEmpty(cedula))
            {
                ViewData["error"] = "La cédula es requerida";
                return View("Index");
            }

            var result = await _usuarioRecuperacionCommandUseCase.GenerarRecuperacionContrasena(cedula);

            // Redirigir a la vista de verificación de código
            TempData["codigoGenerado"] = result;
            TempData["cedula"] = cedula;

            // Usar Redirect con ruta explícita
            return RedirectToAction("VerificarCodigo", "Recuperacion");
        }

        // GET: /Recuperacion/VerificarCodigo
        [HttpGet]
        public IActionResult VerificarCodigo()
        {
            // Verificar que venga de un flujo válido
            if (TempData["cedula"] == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["cedula"] = TempData["cedula"];
            ViewData["codigoGenerado"] = TempData["codigoGenerado"];

            // Mantener los datos para próximo request
            TempData.Keep("cedula");
            TempData.Keep("codigoGenerado");

            return View();
        }

        // POST: /Recuperacion/VerificarCodigo
        [HttpPost]
        public IActionResult VerificarCodigo(string codigoIngresado)
        {
            var codigoGenerado = TempData["codigoGenerado"]?.ToString();
            var cedula = TempData["cedula"]?.ToString();

            if (string.IsNullOrEmpty(codigoGenerado) || string.IsNullOrEmpty(cedula))
            {
                return RedirectToAction("Index");
            }

            if (!string.Equals(codigoIngresado, codigoGenerado, StringComparison.OrdinalIgnoreCase))
            {
                ViewData["error"] = "El código ingresado no es válido";
                ViewData["cedula"] = cedula;
                ViewData["codigoGenerado"] = codigoGenerado;

                TempData.Keep("cedula");
                TempData.Keep("codigoGenerado");

                return View();
            }

            // Código válido - redirigir a cambiar contraseña
            TempData["cedula"] = cedula;
            TempData["codigoVerificado"] = codigoGenerado;

            return RedirectToAction("CambiarContrasena");
        }

        // GET: /Recuperacion/CambiarContrasena
        [HttpGet]
        public IActionResult CambiarContrasena()
        {
            if (TempData["cedula"] == null || TempData["codigoVerificado"] == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["cedula"] = TempData["cedula"];

            // Mantener datos para próximo request
            TempData.Keep("cedula");
            TempData.Keep("codigoVerificado");

            return View();
        }

        // POST: /Recuperacion/CambiarContrasena
        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(string nuevaContrasena, string confirmarContrasena)
        {
            var cedula = TempData["cedula"]?.ToString();
            var codigo = TempData["codigoVerificado"]?.ToString();

            if (string.IsNullOrEmpty(cedula) || string.IsNullOrEmpty(codigo))
            {
                return RedirectToAction("Index");
            }

            // Validaciones
            if (string.IsNullOrEmpty(nuevaContrasena))
            {
                ViewData["error"] = "La nueva contraseña es requerida";
                ViewData["cedula"] = cedula;
                TempData.Keep("cedula");
                TempData.Keep("codigoVerificado");
                return View();
            }

            if (nuevaContrasena != confirmarContrasena)
            {
                ViewData["error"] = "Las contraseñas no coinciden";
                ViewData["cedula"] = cedula;
                TempData.Keep("cedula");
                TempData.Keep("codigoVerificado");
                return View();
            }

            try
            {
                var recoverySave = await _usuarioRecuperacionCommandUseCase.EstablacerRecuperacionContrasena(new UsuarioRecuperacionDto { Cedula = cedula, Codigo = codigo, NuevaContrasena = nuevaContrasena });
                if(recoverySave)
                    //TempData.Keep("Contraseña cambiada correctamente");
                    TempData["SuccessMessage"] = "Contraseña cambiada correctamente";
                // Limpiar TempData
                TempData.Clear();

                // Redirigir a confirmación
                return RedirectToAction("Confirmacion");
            }
            catch (Exception ex)
            {
                ViewData["error"] = "Error al cambiar la contraseña: " + ex.Message;
                ViewData["cedula"] = cedula;
                TempData.Keep("cedula");
                TempData.Keep("codigoVerificado");
                return View();
            }
        }

        // GET: /Recuperacion/Confirmacion
        [HttpGet]
        public IActionResult Confirmacion()
        {
            TempData["SuccessMessage"] = "Contraseña cambiada correctamente";
            return View();
        }
    }
}