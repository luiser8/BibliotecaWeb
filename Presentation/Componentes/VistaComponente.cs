using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

namespace Presentation.Componentes;

    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var user = HttpContext.User;
            var menuItems = new List<NavbarItem>();
            
            // Elementos fijos del menú (siempre visibles si autenticado)
            if (user.Identity?.IsAuthenticated == true)
            {
                // Dashboard (siempre visible)
                menuItems.Add(new NavbarItem
                {
                    Text = "Dashboard",
                    Icon = "fas fa-chart-line",
                    Url = "/Home/Index",
                    IsActive = ViewContext.RouteData.Values["controller"]?.ToString() == "Home"
                });
                
                // Obtener políticas del usuario
                var politicas = GetPoliticasFromClaims(user);
                
                // Generar menú basado en las políticas - mostrar el nombre real de la política
                if (politicas != null && politicas.Any())
                {
                    foreach (var politica in politicas)
                    {
                        // Validar que la política tenga nombre y ruta
                        if (!string.IsNullOrEmpty(politica.Nombre) && !string.IsNullOrEmpty(politica.Ruta))
                        {
                            menuItems.Add(new NavbarItem
                            {
                                Text = politica.Nombre, // Mostrar el nombre real de la política
                                Icon = GetMenuIconFromPolitica(politica.Nombre),
                                Url = GetUrlFromRuta(politica.Ruta),
                                IsActive = IsActiveMenuItem(politica.Ruta)
                            });
                        }
                    }
                }
            }
            
            return View(menuItems);
        }
        
        private List<PoliticaDto> GetPoliticasFromClaims(ClaimsPrincipal user)
        {
            var politicas = new List<PoliticaDto>();
            
            // Primero intentar obtener de claims individuales (más confiable)
            var politicasFromClaims = GetPoliticasFromIndividualClaims(user);
            if (politicasFromClaims != null && politicasFromClaims.Any())
            {
                return politicasFromClaims;
            }
            
            // Si no hay claims individuales, intentar del JSON
            var politicasJson = user.FindFirstValue("PoliticasJson");
            if (!string.IsNullOrEmpty(politicasJson))
            {
                try
                {
                    // El JSON viene serializado con CamelCase (nombre, ruta, politicaId)
                    var options = new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    
                    var politicasUsuario = JsonSerializer.Deserialize<List<PoliticasUsuario>>(politicasJson, options);
                    if (politicasUsuario != null && politicasUsuario.Any())
                    {
                        politicas = politicasUsuario
                            .Where(p => !string.IsNullOrEmpty(p.Nombre) && !string.IsNullOrEmpty(p.Ruta))
                            .Select(p => new PoliticaDto
                            {
                                Nombre = p.Nombre ?? "",
                                Ruta = p.Ruta ?? ""
                            }).ToList();
                    }
                }
                catch
                {
                    // Si falla, retornar lista vacía
                    return new List<PoliticaDto>();
                }
            }
            
            return politicas ?? new List<PoliticaDto>();
        }
        
        private List<PoliticaDto> GetPoliticasFromIndividualClaims(ClaimsPrincipal user)
        {
            var politicas = new List<PoliticaDto>();
            var politicaClaims = user.FindAll("Politica");
            
            foreach (var claim in politicaClaims)
            {
                var parts = claim.Value.Split('|');
                if (parts.Length == 2)
                {
                    politicas.Add(new PoliticaDto
                    {
                        Nombre = parts[0],
                        Ruta = parts[1]
                    });
                }
            }
            
            return politicas;
        }
        
        private string GetMenuTextFromPolitica(string politicaNombre)
        {
            // Mapear nombres de políticas a texto de menú
            return politicaNombre switch
            {
                "LibrosConsultar" => "Catálogo",
                "LibrosCrear" => "Crear Libro",
                "LibrosEditar" => "Editar Libros",
                "LibrosPrestar" => "Prestar Libro",
                "LibrosDevolver" => "Devoluciones",
                "PrestamosConsultar" => "Préstamos",
                "PrestamosCrear" => "Nuevo Préstamo",
                "PrestamosReportes" => "Reportes Préstamos",
                "UsuariosConsultar" => "Usuarios",
                "UsuariosCrear" => "Crear Usuario",
                "CatalogosConsultar" => "Catálogos",
                "ExtensionesConsultar" => "Extensiones",
                "CarrerasConsultar" => "Carreras",
                "PerfilConsultar" => "Mi Perfil",
                "PerfilEditar" => "Editar Perfil",
                "ReportesConsultar" => "Reportes",
                _ => politicaNombre.Replace("Consultar", "").Trim()
            };
        }
        
        private string GetMenuIconFromPolitica(string politicaNombre)
        {
            // Mapear nombres de políticas a iconos
            return politicaNombre switch
            {
                string s when s.Contains("Libros") => "fas fa-book",
                string s when s.Contains("Prestamos") => "fas fa-book-reader",
                string s when s.Contains("Usuarios") => "fas fa-users",
                string s when s.Contains("Catalogo") => "fas fa-search",
                string s when s.Contains("Extension") => "fas fa-building",
                string s when s.Contains("Carrera") => "fas fa-graduation-cap",
                string s when s.Contains("Perfil") => "fas fa-user",
                string s when s.Contains("Reportes") => "fas fa-chart-bar",
                _ => "fas fa-cog"
            };
        }
        
        private string GetUrlFromRuta(string ruta)
        {
            // Convertir ruta "Modulo/Accion" a URL "/Modulo/Accion"
            if (string.IsNullOrEmpty(ruta))
                return "#";
            
            return $"/{ruta}";
        }
        
        private bool IsActiveMenuItem(string ruta)
        {
            if (string.IsNullOrEmpty(ruta))
                return false;
            
            var currentController = ViewContext.RouteData.Values["controller"]?.ToString();
            var currentAction = ViewContext.RouteData.Values["action"]?.ToString();
            
            var parts = ruta.Split('/');
            if (parts.Length >= 2)
            {
                var controller = parts[0];
                var action = parts[1];
                
                return currentController?.Equals(controller, StringComparison.OrdinalIgnoreCase) == true &&
                       currentAction?.Equals(action, StringComparison.OrdinalIgnoreCase) == true;
            }
            
            return false;
        }
    }
    
    public class NavbarItem
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
    
    public class PoliticaDto
    {
        public string Nombre { get; set; }
        public string Ruta { get; set; }
    }
