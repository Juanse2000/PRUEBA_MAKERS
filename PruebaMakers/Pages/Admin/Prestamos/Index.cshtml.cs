using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PruebaMakers.Pages.Admin.Prestamos
{
    public class IndexModel : PageModel
    {
        private readonly IPrestamoService _prestamoService;
        private readonly PRUEBA_MAKERS_DBCONTEXT _context;

        public IndexModel(IPrestamoService prestamoService, PRUEBA_MAKERS_DBCONTEXT context)
        {
            _prestamoService = prestamoService;
            _context = context;
        }

        public List<PrestamoVistaDto> PrestamosPendientes { get; set; } = new();
        public List<PrestamoVistaDto> PrestamosProcesados { get; set; } = new();
        public async Task OnGetAsync()
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Estado)
                .OrderByDescending(p => p.FechaSolicitud)
                .ToListAsync();

            PrestamosPendientes = prestamos
                .Where(p => p.Estado.Nombre == "Pendiente")
                .Select(p => MapearPrestamo(p))
                .ToList();

            PrestamosProcesados = prestamos
                .Where(p => p.Estado.Nombre != "Pendiente")
                .Select(p => MapearPrestamo(p))
                .ToList();
        }


        public async Task<IActionResult> OnPostCambiarEstadoAsync(int id, string nuevoEstado)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var prestamo = await _context.Prestamos
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
                return NotFound();

            var estadoNuevo = await _context.EstadosPrestamo
                .FirstOrDefaultAsync(e => e.Nombre == nuevoEstado);

            if (estadoNuevo == null)
                return BadRequest("Estado inválido.");

            prestamo.EstadoId = estadoNuevo.Id;
            prestamo.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return RedirectToPage();
        }


        private PrestamoVistaDto MapearPrestamo(Prestamo p) => new PrestamoVistaDto
        {
            Id = p.Id,
            Monto = p.Monto,
            PlazoEnMeses = p.PlazoEnMeses,
            Estado = p.Estado.Nombre,
            FechaSolicitud = p.FechaSolicitud,
            FechaActualizacion = p.FechaActualizacion,
            UsuarioNombre = p.Usuario.Nombre,
            UsuarioEmail = p.Usuario.Email
        };
    }
}
