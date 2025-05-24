using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PrestamoSolicitudDto
    {
        public decimal Monto { get; set; }
        public int PlazoMeses { get; set; }
        public int UsuarioId { get; set; }
    }
}
