using Application.DTOs;
using Application.Interfaces;
using Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace PruebaMakers.Pages.Prestamos
{
    public class SolicitarPrestamoModel : PageModel
    {
        private readonly IMemoryCache _cache;
        private readonly IPrestamoService _prestamoService;
        private readonly PRUEBA_MAKERS_DBCONTEXT _context;

        public SolicitarPrestamoModel(IPrestamoService prestamoService, PRUEBA_MAKERS_DBCONTEXT context, IMemoryCache cache)
        {
            _prestamoService = prestamoService;
            _context = context;
            _cache = cache;
        }

        [BindProperty]
        public PrestamoSolicitudDto Prestamo { get; set; }
        public List<PrestamoVistaDto> MisPrestamos { get; set; } = new();

        public string Mensaje { get; set; }
        public async Task OnGetAsync()
        {
            int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            MisPrestamos = await _context.Prestamos
                .Where(p => p.UsuarioId == usuarioId)
                .Include(p => p.Estado)
                .OrderByDescending(p => p.FechaSolicitud)
                .Select(p => new PrestamoVistaDto
                {
                    Monto = p.Monto,
                    PlazoEnMeses = p.PlazoEnMeses,
                    Estado = p.Estado.Nombre,
                    FechaSolicitud = p.FechaSolicitud
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // cargar lista actualizada si hay error de validación
                return Page();
            }

            Prestamo.UsuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _prestamoService.SolicitarPrestamoAsync(Prestamo);

            Mensaje = "Solicitud enviada correctamente.";
            ModelState.Clear(); 

            await OnGetAsync(); 

            return Page(); 
        }
    }
}
