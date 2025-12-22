// Controllers/BaseController.cs

using Application.Interfaces;
using Application.UseCases.Politicas;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Presentation.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IPoliticasUsuarioUseCase _politicasUsuarioUseCase;

        protected BaseController(IOptions<AppSettings> appSettings, IPoliticasUsuarioUseCase politicasUsuarioUseCase)
        {
            _appSettings = appSettings?.Value;
            _politicasUsuarioUseCase = politicasUsuarioUseCase;
        }

        /// <summary>
        /// Establece los ViewData desde la configuración de la aplicación
        /// </summary>
        protected void SetViewDataFromSettings()
        {
            if (_appSettings == null)
            {
                // Valores por defecto si no hay configuración
                SetDefaultViewData();
                return;
            }

            // Configuración general
            ViewData["SiteTitle"] = _appSettings.SiteTitle;
            ViewData["LibraryName"] = _appSettings.LibraryName;
            ViewData["LibraryLogo"] = _appSettings.LibraryLogo;
            ViewData["LibraryLogoAlt"] = _appSettings.LibraryLogoAlt;
            ViewData["LibrarySlogan"] = _appSettings.LibrarySlogan;
            ViewData["FooterText"] = _appSettings.FooterText;
            ViewData["SupportEmail"] = _appSettings.SupportEmail;
            ViewData["SupportPhone"] = _appSettings.SupportPhone;
            ViewData["LibraryHours"] = _appSettings.LibraryHours;

            // Items de navegación
            ViewData["NavbarItems"] = _appSettings.NavbarItems;
            ViewData["AdminNavbarItems"] = _appSettings.AdminNavbarItems;
            ViewData["QuickLinks"] = _appSettings.QuickLinks;
        }

        /// <summary>
        /// Establece valores por defecto para ViewData
        /// </summary>
        private void SetDefaultViewData()
        {
            ViewData["SiteTitle"] = "Sistema de Biblioteca";
            ViewData["LibraryName"] = "Biblioteca Universitaria";
            ViewData["LibraryLogo"] = "/images/library-logo.png";
            ViewData["LibraryLogoAlt"] = "Logo Biblioteca";
            ViewData["LibrarySlogan"] = "Conocimiento al alcance de todos";
            ViewData["FooterText"] = "© 2025 Biblioteca Universitaria - Todos los derechos reservados";
            ViewData["SupportEmail"] = "biblioteca@universidad.edu";
            ViewData["SupportPhone"] = "+1 (555) 123-4567";
            ViewData["LibraryHours"] = "Lunes a Viernes: 8:00 AM - 8:00 PM | Sábados: 9:00 AM - 2:00 PM";

            // Items de navegación por defecto
            ViewData["NavbarItems"] = new List<NavbarItem>
            {
                new NavbarItem { Text = "Inicio", Controller = "Home", Action = "Index", Icon = "fas fa-home" },
                new NavbarItem { Text = "Catálogo", Controller = "Catalogo", Action = "Index", Icon = "fas fa-book" },
                new NavbarItem { Text = "Mis Préstamos", Controller = "Prestamos", Action = "Index", Icon = "fas fa-book-reader" }
            };

            ViewData["AdminNavbarItems"] = new List<NavbarItem>
            {
                new NavbarItem { Text = "Dashboard", Controller = "Admin", Action = "Dashboard", Icon = "fas fa-chart-line" },
                new NavbarItem { Text = "Gestión Libros", Controller = "Admin", Action = "Libros", Icon = "fas fa-book-medical" }
            };

            ViewData["QuickLinks"] = new List<QuickLink>
            {
                new QuickLink { Text = "Reglamento", Url = "/reglamento", Icon = "fas fa-file-alt" },
                new QuickLink { Text = "Recursos Digitales", Url = "/recursos", Icon = "fas fa-laptop" }
            };
        }

        /// <summary>
        /// Establece el breadcrumb para la vista actual
        /// </summary>
        protected void SetBreadcrumb(params (string Text, string Action, string Controller)[] items)
        {
            if (items == null || items.Length == 0) return;

            var breadcrumbHtml = "";
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (string.IsNullOrEmpty(item.Action))
                {
                    // Último item (activo)
                    breadcrumbHtml += $@"<li class=""breadcrumb-item active"" aria-current=""page"">{item.Text}</li>";
                }
                else
                {
                    // Item con enlace
                    breadcrumbHtml += $@"<li class=""breadcrumb-item""><a asp-controller=""{item.Controller}"" asp-action=""{item.Action}"">{item.Text}</a></li>";
                }
            }

            ViewData["Breadcrumb"] = breadcrumbHtml;
        }

        /// <summary>
        /// Redirige con mensaje de éxito
        /// </summary>
        protected IActionResult RedirectWithSuccess(string action, string controller, string message)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction(action, controller);
        }

        /// <summary>
        /// Redirige con mensaje de error
        /// </summary>
        protected IActionResult RedirectWithError(string action, string controller, string message)
        {
            TempData["ErrorMessage"] = message;
            return RedirectToAction(action, controller);
        }

        /// <summary>
        /// Verifica si el usuario actual tiene un rol específico
        /// </summary>
        protected bool UserHasRole(string role)
        {
            return User?.IsInRole(role) ?? false;
        }

        /// <summary>
        /// Obtiene el ID del usuario actual desde los claims
        /// </summary>
        protected int? GetCurrentUserId()
        {
            var userIdClaim = User?.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}