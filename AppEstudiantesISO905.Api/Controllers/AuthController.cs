using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AppEstudiantesISO905.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUsuarioHandler _loginHandler;

        public AuthController(ILoginUsuarioHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }

        // Endpoint abierto para autenticarse
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthorizationModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Datos de inicio de sesión inválidos.");

            try
            {
                var token = await _loginHandler.Handle(request.Email, request.Password);

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Usuario o contraseña incorrectos.");

                // Retorna el token en el body
                return Ok(new { Token = token });
            }
                catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al iniciar sesión.", Details = ex.Message });
            }
        }
    }
}
