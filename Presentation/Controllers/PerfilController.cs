using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class PerfilController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}