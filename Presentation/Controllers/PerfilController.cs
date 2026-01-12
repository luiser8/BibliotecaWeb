using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

public class PerfilController : Controller
{
    public IActionResult Index()
    {
        // Obtener datos del usuario para personalizar la vista
        var nombreUsuario = User?.FindFirstValue("NombreCompleto")
                            ?? User?.Identity?.Name
                            ?? "Usuario";

        var rolUsuario = User?.FindFirstValue("Rol")
                         ?? User?.FindFirst(ClaimTypes.Role)?.Value
                         ?? "Usuario";

        // Puedes acceder a las pol�ticas ya cargadas por el filter
        var politicasBasicas = ViewData["PoliticasBasicas"] as IEnumerable<dynamic>
                               ?? Enumerable.Empty<dynamic>();

        ViewData["NombreUsuario"] = nombreUsuario;
        ViewData["RolUsuario"] = rolUsuario;
        ViewData["PoliticasCount"] = politicasBasicas.Count();

        return View();
    }
}