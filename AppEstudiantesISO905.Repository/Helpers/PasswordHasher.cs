using AppEstudiantesISO905.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Repository.Helpers
{
    public class PasswordHasher
    {
        public static string HashPassword(Usuario usuario)
        {
            var passwordHasher = new PasswordHasher<Usuario>();

            usuario.PasswordHash = passwordHasher.HashPassword(usuario, usuario.PasswordHash);

            return usuario.PasswordHash;
        }
    }
}
