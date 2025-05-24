using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EstadoPrestamo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public List<Prestamo> Prestamos { get; set; } = new();
    }
}
