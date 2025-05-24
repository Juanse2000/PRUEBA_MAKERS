using Application.DTOs;
using Application.Interfaces;
using Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PruebaMakers.Pages.Usuarios
{
    public class RegistroModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;
        private readonly PRUEBA_MAKERS_DBCONTEXT  _context;

        public RegistroModel(IUsuarioService usuarioService, PRUEBA_MAKERS_DBCONTEXT context)
        {
            _usuarioService = usuarioService;
            _context = context;
        }

        [BindProperty]
        public UsuarioRegistroDto Usuario { get; set; } = new();
        public List<SelectListItem> Roles { get; set; }

        public string? Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            Roles = await _context.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Nombre
                }).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // recargar roles si hay error
                return Page();
            }

            try
            {
                await _usuarioService.RegistrarUsuarioAsync(Usuario);
                Mensaje = "Usuario registrado exitosamente";
                return RedirectToPage("/Usuarios/Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await OnGetAsync(); // recargar roles en caso de excepción
                return Page();
            }
        }
    }
}
