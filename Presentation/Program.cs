using Application.Domain.Configuration;
using Application.Interfaces;
using Application.UseCases.Carreras;
using Application.UseCases.ExtensionQuery;
using Application.UseCases.Perfil;
using Application.UseCases.Politicas;
using Application.UseCases.Usuarios;
using Application.UseCases.Email;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Configurations;
using Infrastructure.Email;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Presentation.Filters;
using Presentation.Middleware;
using Presentation.Services;
using Application.UseCases.UsuarioRecuperacion;

var builder = WebApplication.CreateBuilder(args);

// Agregar configuración de AppSettings
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("Security"));

// Agregar servicios de controladores con vistas
builder.Services.AddControllersWithViews();

// Agregar sesión (ESENCIAL para almacenar recursos)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.IsEssential = true;
});

// Agregar autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.LogoutPath = "/Usuario/Logout";
        options.AccessDeniedPath = "/Usuario/AccesoDenegado";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    var availableConnections = builder.Configuration.GetSection("ConnectionStrings").GetChildren();
    var connectionNames = string.Join(", ", availableConnections.Select(c => c.Key));
    throw new InvalidOperationException(
        $"Connection string 'DefaultConnection' not found in configuration. " +
        $"Available connection strings: {connectionNames}"
    );
}

// Configurar filtros
builder.Services.AddScoped<CargarPoliticasFilter>();
builder.Services.AddScoped<ValidarAccesoFilter>(); // Nuevo filtro

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CargarPoliticasFilter>();
    options.Filters.Add<ValidarAccesoFilter>(); // Filtro de validación global
});

// Registrar servicios de infraestructura
builder.Services.AddScoped<IConnectionFactory>(_ => new ConnectionFactory(connectionString));
builder.Services.AddScoped<IDataTableExecute, DataTableExecute>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Repositorios
builder.Services.AddScoped<ICarreraRepository, CarreraRepository>();
builder.Services.AddScoped<IExtensionRepository, ExtensionRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IDatosPersonalesRepository, DatosPersonalesRepository>();
builder.Services.AddScoped<IDatosAcademicosRepository, DatosAcademicosRepository>();
builder.Services.AddScoped<IPoliticasUsuarioRepository, PoliticasUsuarioRepository>();
builder.Services.AddScoped<IUsuarioPerfilRepository, UsuarioPerfilRepository>();
builder.Services.AddScoped<IEmailPort, EmailAdapter>();
builder.Services.AddScoped<ICodigoRecuperacion, CodigoRecuperacion>();

// Casos de uso
builder.Services.AddScoped<IExtensionQueryUseCase, ExtensionQueryUseCase>();
builder.Services.AddScoped<ICarrerasQueryUseCase, CarrerasQueryUseCase>();
builder.Services.AddScoped<IUsuarioQueryUseCase, UsuarioQueryUseCase>();
builder.Services.AddScoped<IUsuarioCommandUseCase, UsuarioCommandUseCase>();
builder.Services.AddScoped<IPoliticasUsuariosQueryUseCase, PoliticasUsuariosQueryUseCase>();
builder.Services.AddScoped<IUsuarioPerfilCommandUseCase, UsuarioPerfilCommandUseCase>();
builder.Services.AddScoped<IAuthQueryUseCase, AuthQueryUseCase>();
builder.Services.AddScoped<IEmailCommandUseCase, EmailCommandUseCase>();
builder.Services.AddScoped<IUsuarioRecuperacionCommandUseCase, UsuarioRecuperacionCommandUseCase>();

// Otros servicios
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ExceptionHandlerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// IMPORTANTE: El orden correcto
app.UseSession(); // <-- Agregar esto ANTES de Authentication
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AccesoMiddleware>();
app.UseMiddleware<RutaInvalidaMiddleware>();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Usuario}/{action=Login}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}",
    constraints: new { id = @"\d*" }
).WithStaticAssets();

app.MapControllerRoute(
    name: "catchall",
    pattern: "{*url}",
    defaults: new { controller = "Home", action = "Error" }
);

app.MapFallbackToController("Error", "Home");
await app.RunAsync();