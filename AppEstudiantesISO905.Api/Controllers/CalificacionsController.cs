using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AppEstudiantesISO905.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CalificacionsController : ControllerBase
    {
        private readonly ICalificacionService _calificacionService;
        private readonly IEstudianteService _estudianteService;
        private readonly IMateriaService _materiaService;

        public CalificacionsController(ICalificacionService calificacionService, IEstudianteService estudianteService, IMateriaService materiaService)
        {   
            _calificacionService = calificacionService;
            _estudianteService = estudianteService;
            _materiaService = materiaService;
        }

        // GET: api/Calificacions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var calificaciones = await _calificacionService.GetAllAsync();
            return Ok(calificaciones);
        }

        // GET: api/Calificacions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var calificacion = await _calificacionService.GetByIdAsync(id);
            if (calificacion == null)
                return NotFound($"No se encontró la calificación con ID {id}");

            return Ok(calificacion);
        }

        // POST: api/Calificacions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CalificacionCreateVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var calificacion = new Calificacion
            {
                EstudianteId = model.EstudianteId,
                MateriaId = model.MateriaId,
                Calificacion1 = model.Calificacion1,
                Calificacion2 = model.Calificacion2,
                Calificacion3 = model.Calificacion3,
                Calificacion4 = model.Calificacion4,
                Examen = model.Examen,
                Clasificacion = "Pendiente",
                Estado = "Activo"
            };

            await _calificacionService.AddAsync(calificacion);
            return CreatedAtAction(nameof(GetById), new { id = calificacion.Id }, calificacion);
        }

        // PUT: api/Calificacions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CalificacionCreateVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = await _calificacionService.GetByIdAsync(id);
            if (existente == null)
                return NotFound($"No se encontró la calificación con ID {id}");

            await _calificacionService.UpdateAsync(id, model);
            return NoContent();
        }

        // DELETE: api/Calificacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existente = await _calificacionService.GetByIdAsync(id);
            if (existente == null)
                return NotFound($"No se encontró la calificación con ID {id}");

            await _calificacionService.DeleteAsync(id);
            return NoContent();
        }

        // GET: api/Calificacions/Combos
        [HttpGet("Combos")]
        public async Task<IActionResult> GetCombos()
        {
            var estudiantes = await _estudianteService.GetAllAsync();
            var materias = await _materiaService.GetAllMateriasAsync();

            var estudiantesCombo = estudiantes.Select(e => new
            {
                e.EstudianteId,
                NombreCompleto = $"{e.Apellido} {e.Nombre}"
            });

            var resultado = new
            {
                Estudiantes = estudiantesCombo,
                Materias = materias.Select(m => new { m.MateriaId, m.Nombre })
            };

            return Ok(resultado);
        }

        [HttpGet("ExportarToCSV")]
        public async Task<IActionResult> ExportToCsv()
        {
            try
            {
                var csvBytes = await ToCsvData();
                return File(csvBytes, "text/csv", "Calificaciones.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [NonAction]
        private async Task<byte[]> ToCsvData()
        {
            var calificacionsData = await _calificacionService.ExportToCsvAsync();

            var csv = new StringBuilder();
            csv.AppendLine("Calificacion1,Calificacion2,Calificacion3,Calificacion4,Examen,PromedioCalificaciones,TotalCalificacion,Clasificacion,Estado,Estudiante,Matricula,Materia");

            foreach (var c in calificacionsData)
            {
                csv.AppendLine($"{c.Calificacion1},{c.Calificacion2},{c.Calificacion3},{c.Calificacion4},{c.Examen},{c.PromedioCalificaciones},{c.TotalCalificacion},{c.Clasificacion},{c.Estado},{c.Estudiante?.Nombre} {c.Estudiante?.Apellido},{c.Estudiante?.Matricula},{c.Materia?.Nombre}");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }
    }
}
