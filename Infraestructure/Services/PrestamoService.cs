using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services
{
    public class PrestamoService : IPrestamoService
    {
        private readonly PRUEBA_MAKERS_DBCONTEXT _context;

        public PrestamoService(PRUEBA_MAKERS_DBCONTEXT context)
        {
            _context = context;
        }

        public async Task<bool> SolicitarPrestamoAsync(PrestamoSolicitudDto dto)
        {
            if (dto.Monto <= 0 || dto.PlazoMeses <= 0)
                return false;

            var prestamo = new Prestamo
            {
                Monto = dto.Monto,
                PlazoEnMeses = dto.PlazoMeses,
                UsuarioId = dto.UsuarioId,
                EstadoId = 1, 
                FechaSolicitud = DateTime.UtcNow
            };

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
