using Application.Interfaces;
using Application.UseCases.Carreras;
using Application.UseCases.Extension;
using Application.UseCases.Usuarios;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar configuración de AppSettings
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

// Agregar servicios de controladores con vistas
builder.Services.AddControllersWithViews();

// Agregar autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.LogoutPath = "/Usuario/Logout";
        options.AccessDeniedPath = "/Usuario/AccesoDenegado";
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // Mensaje más descriptivo para debugging
    var availableConnections = builder.Configuration.GetSection("ConnectionStrings").GetChildren();
    var connectionNames = string.Join(", ", availableConnections.Select(c => c.Key));

    throw new InvalidOperationException(
        $"Connection string 'DefaultConnection' not found in configuration. " +
        $"Available connection strings: {connectionNames}"
    );
}

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IConnectionFactory>(provider => new ConnectionFactory(connectionString));
builder.Services.AddScoped<IDataTableExecute, DataTableExecute>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<ICarreraRepository, CarreraRepository>();
builder.Services.AddScoped<IExtensionRepository, ExtensionRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IDatosPersonalesRepository, DatosPersonalesRepository>();
builder.Services.AddScoped<IDatosAcademicosRepository, DatosAcademicosRepository>();

builder.Services.AddScoped<IExtensionQueryUseCase, ExtensionQueryUseCase>();
builder.Services.AddScoped<ICarrerasQueryUseCase, CarrerasUseCase>();
builder.Services.AddScoped<IUsuarioQueryUseCase, UsuarioQueryUseCase>();
builder.Services.AddScoped<IUsuarioCommandUseCase, UsuarioCommandUseCase>();
builder.Services.AddScoped<IAuthUseCase, AuthUseCase>();

// Servicios de presentación
builder.Services.AddScoped<ExceptionHandlerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Usuario}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();