using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Controllers;

public class HomeController : BaseController
{
    private readonly AppSettings _appSettings;
    public HomeController(IOptions<AppSettings> appSettings) : base(appSettings)
    {
        _appSettings = appSettings?.Value;
    }

    public IActionResult Index()
    {
        SetViewDataFromSettings();
        SetBreadcrumb(("Inicio", "Index", "Home"));
        
        // Obtener datos para el dashboard
        var viewModel = new DashBoard
        {
            TotalBooks = 12500,
            AvailableBooks = 8500,
            ActiveLoans = 3200,
            PendingReturns = 150
        };
        
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        SetViewDataFromSettings();
        SetBreadcrumb(
            ("Inicio", "Index", "Home"),
            ("Privacidad", "", "")
        );
        return View();
    }

    public IActionResult Contact()
    {
        SetViewDataFromSettings();
        SetBreadcrumb(
            ("Inicio", "Index", "Home"),
            ("Contacto", "", "")
        );
        return View();
    }
}