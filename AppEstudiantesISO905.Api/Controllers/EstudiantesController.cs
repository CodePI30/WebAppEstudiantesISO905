using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AppEstudiantesISO905.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudianteService _service;

        public EstudiantesController(IEstudianteService service)
        {
            _service = service;
        }

        [HttpGet("GetEstudiantes")]
        public async Task<IActionResult> GetEstudiantes()
        {
            var estudiantes = await _service.GetAllAsync();
            return Ok(estudiantes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstudiante(int id)
        {
            var estudiante = await _service.GetByIdAsync(id);
            if (estudiante == null)
                return NotFound(new { message = "Estudiante no encontrado" });

            return Ok(estudiante);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(EstudianteCreateModel estudiante)
        {
            try
            {
                var estudianteLastSequence = await _service.GetLastSequence();

                estudiante.Matricula = estudianteLastSequence.Matricula;

                await _service.AddAsync(estudiante);

                return NoContent();
            }
            catch (Exception)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al crear el estudiante.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEstudiante(int id, EstudianteCreateModel estudiante)
        {
            if (id != estudiante.EstudianteId)
                return BadRequest(new { message = "ID de estudiante no coincide" });

            try
            {
                await _service.UpdateAsync(estudiante);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al actualizar el estudiante.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            if (id is 0)
                return StatusCode((int)HttpStatusCode.BadRequest, "ID de estudiante no válido.");

            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
