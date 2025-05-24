using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Application.Interfaces;
using Infraestructure.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = string.Empty;

connectionString = builder.Configuration.GetConnectionString("Connection");

// Add services to the container.
builder.Services.AddRazorPages();

/* BD Configurar el DbContext usando la cadena de conexión de Key Vault */
builder.Services.AddDbContext<PRUEBA_MAKERS_DBCONTEXT>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPrestamoService, PrestamoService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.AccessDeniedPath = "/Usuarios/AccesoDenegado";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
