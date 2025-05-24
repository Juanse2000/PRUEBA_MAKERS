using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Data
{
    public class PRUEBA_MAKERS_DBCONTEXT : DbContext
    {
        public PRUEBA_MAKERS_DBCONTEXT(DbContextOptions<PRUEBA_MAKERS_DBCONTEXT> options) : base(options) 
        {

        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<UsuarioRol> UsuariosRoles => Set<UsuarioRol>();
        public DbSet<Prestamo> Prestamos => Set<Prestamo>();
        public DbSet<EstadoPrestamo> EstadosPrestamo => Set<EstadoPrestamo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioRol>().HasKey(ur => new { ur.UsuarioId, ur.RolId });
        }
    }
}
