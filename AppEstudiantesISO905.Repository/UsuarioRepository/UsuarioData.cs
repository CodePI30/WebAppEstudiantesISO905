using AppEstudiantesISO905.Domain.Contracts;
using AppEstudiantesISO905.Domain.ExceptionManager;
using AppEstudiantesISO905.Domain.Models;
using AppEstudiantesISO905.Repository.Helpers;
using AppEstudiantesISO905.Repository.WebAppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Repository.UsuarioRepository
{
    public class UsuarioData : IUsuarioData
    {
        private readonly EstudiantesIso905Context _context;
        public UsuarioData(EstudiantesIso905Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuarios(string? email = null)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                return await _context.Usuarios
                    .Where(u => u.Email == email)
                    .ToListAsync();
            }

            return await _context.Usuarios.ToListAsync();
        }
        public async Task<Usuario?> ObtenerUsuariosByEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ExceptionApp("El email no puede ser nulo o vacío");
            }

            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task CrearUsuario(Usuario usuario)
        {
            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (usuarioExistente is not null)
                throw new ExceptionApp("Ya existe un usuario con el email indicado");

            usuario.PasswordHash = PasswordHasher.HashPassword(usuario);

            _context.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> ObtenerUsuarioForActions(int id)
        {
            var usuario = await ObtenerUsuarioById(id);

            if (usuario is null)
                throw new ExceptionApp("El usuario no existe");

            return usuario;
        }

        public async Task EditarUsuario(Usuario usuario)
        {
            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UsuarioExists(usuario.UsuarioId))
                {
                    throw new NotFoundException("No se ha encontrado el usuario indicado en nuestra base de datos", ex);
                }
                else
                {
                    throw new ExceptionApp("Error al editar el usuario", ex);
                }
            }
        }

        public async Task EliminarUsuario(int id)
        {
            var usuario = await ObtenerUsuarioById(id);

            if (usuario is null)
                throw new NotFoundException("No se ha encontrado el usuario indicado en nuestra base de datos");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> ObtenerUsuarioById(int? id)
        {
            Usuario? usuario = await _context.Usuarios.FindAsync(id);

            if (usuario is null)
                throw new NotFoundException("No se ha encontrado el usuario indicado en nuestra base de datos");

            return usuario;
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
