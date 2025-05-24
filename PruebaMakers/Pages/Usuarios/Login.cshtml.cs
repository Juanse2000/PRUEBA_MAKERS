using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Domain.Entities;

namespace PruebaMakers.Pages.Usuarios
{
    public class LoginModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public LoginModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        [BindProperty]
        public LoginDto Login { get; set; } = new();

        public string? Mensaje { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _usuarioService.LoginAsync(Login);

            if (user == null)
            {
                Mensaje = "Credenciales inválidas.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var rol in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


            if (user.Roles.Contains("Administrador"))
                return RedirectToPage("/Admin/Prestamos/Index");

            return RedirectToPage("/Prestamos/SolicitarPrestamo");
        }
    }
}
