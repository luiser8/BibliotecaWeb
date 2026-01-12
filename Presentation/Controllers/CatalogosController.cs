using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
    public class CatalogosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
