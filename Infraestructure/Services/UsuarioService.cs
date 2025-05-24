using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly PRUEBA_MAKERS_DBCONTEXT _context;

        public UsuarioService(PRUEBA_MAKERS_DBCONTEXT context)
        {
            _context = context;
        }

        public async Task<int> RegistrarUsuarioAsync(UsuarioRegistroDto dto)
        {
            var passwordHash = HashPassword(dto.Password);

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = passwordHash,
            };

            // Buscar el rol por ID
            var rol = await _context.Roles.FindAsync(dto.RolId);
            if (rol == null)
                throw new Exception("Rol inválido.");

            usuario.Roles.Add(new UsuarioRol
            {
                Usuario = usuario,
                Rol = rol
            });

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario.Id;
        }

        public async Task<UsuarioDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Usuarios
                .Include(u => u.Roles).ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
                return null;

            return new UsuarioDto
            {
                Id = user.Id,
                Email = user.Email,
                Nombre = user.Nombre,
                Roles = user.Roles.Select(r => r.Rol.Nombre).ToList()
            };
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
