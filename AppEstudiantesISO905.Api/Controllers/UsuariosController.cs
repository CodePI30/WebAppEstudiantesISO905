using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Domain.ExceptionManager;
using AppEstudiantesISO905.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppEstudiantesISO905.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _service.ObtenerUsuarios();
            return Ok(usuarios);
        }

        // GET: api/usuarios/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            try
            {
                var usuario = await _service.ObtenerUsuarioById(id);
                if (usuario == null)
                    return NotFound();

                return Ok(usuario);
            }
            catch (ExceptionApp ex)
            {
                return BadRequest(new { Message = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.CrearUsuario(usuario);
                return Ok(new { Message = "Usuario creado exitosamente." });
            }
            catch (ExceptionApp ex)
            {
                return BadRequest(new { Message = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // PUT: api/usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.EditarUsuario(id, usuario);
                return Ok(new { Message = "Usuario actualizado correctamente." });
            }
            catch (NotFoundException)
            {
                return NotFound(new { Message = "Usuario no encontrado." });
            }
            catch (ExceptionApp ex)
            {
                return BadRequest(new { Message = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // DELETE: api/usuarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                await _service.EliminarUsuario(id);
                return Ok(new { Message = "Usuario eliminado correctamente." });
            }
            catch (NotFoundException)
            {
                return NotFound(new { Message = "Usuario no encontrado." });
            }
            catch (ExceptionApp ex)
            {
                return BadRequest(new { Message = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
