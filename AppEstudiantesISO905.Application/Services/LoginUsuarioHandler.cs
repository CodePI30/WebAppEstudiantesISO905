using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Domain.Contracts;
using AppEstudiantesISO905.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Application.Services
{
    public class LoginUsuarioHandler : ILoginUsuarioHandler
    {
        private readonly IUsuarioData _usuarioRepo;
        private readonly IJwtTokenService _jwtService;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public LoginUsuarioHandler(IUsuarioData usuarioRepo, IJwtTokenService jwtService)
        {
            _usuarioRepo = usuarioRepo;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<string> Handle(string email, string password)
        {
            var usuariosList = await _usuarioRepo.ObtenerUsuarios(email);

            var usuario = usuariosList.FirstOrDefault();

            if (usuario == null)
                throw new UnauthorizedAccessException("Usuario no encontrado.");

            var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            return _jwtService.GenerateToken(usuario);
        }
    }
}
