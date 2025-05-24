using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public decimal Monto { get; set; }
        public int PlazoEnMeses { get; set; }
        public int EstadoId { get; set; }
        public EstadoPrestamo Estado { get; set; } = null!;
        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
    }
}
