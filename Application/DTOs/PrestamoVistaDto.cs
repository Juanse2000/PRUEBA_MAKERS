using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PrestamoVistaDto
    {
        public int Id { get; set; } 
        public decimal Monto { get; set; }
        public int PlazoEnMeses { get; set; }
        public string Estado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioEmail { get; set; }
    }
}
