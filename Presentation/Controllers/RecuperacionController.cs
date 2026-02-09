using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class RecuperacionController : Controller
    {
        private readonly IUsuarioRecuperacionCommandUseCase _usuarioRecuperacionCommandUseCase;

        public RecuperacionController(IUsuarioRecuperacionCommandUseCase usuarioRecuperacionCommandUseCase)
        {
            _usuarioRecuperacionCommandUseCase = usuarioRecuperacionCommandUseCase;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET:  /Usuario/Recuperacion
        public async Task<IActionResult> Recuperacion()
        {
            var codigo = _usuarioRecuperacionCommandUseCase.GenerarRecuperacionContrasena();
            ViewData["codigo"] = codigo;
            return View();
        }

        //POST:  /Usuario/Recuperar
        [HttpPost]
        public async Task<IActionResult> Recuperar()
        {
            var codigo = _usuarioRecuperacionCommandUseCase.GenerarRecuperacionContrasena();
            ViewData["codigo"] = codigo;
            return View();
        }
    }
}
