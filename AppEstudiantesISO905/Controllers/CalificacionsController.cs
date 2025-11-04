using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppEstudiantesISO905.Domain.Models;
using AppEstudiantesISO905.Repository.WebAppDbContext;
using AppEstudiantesISO905.Application.Contracts;
using DocumentFormat.OpenXml.InkML;
using System.Text;
using AppEstudiantesISO905.Application.Services;

namespace AppEstudiantesISO905.Controllers
{
    public class CalificacionsController : Controller
    {
        private readonly ICalificacionService _service;
        private readonly IEstudianteService _estudianteService;
        private readonly IMateriaService _materiaService;

        public CalificacionsController(
            ICalificacionService service,
            IEstudianteService estudianteService,
            IMateriaService materiaService)
        {
            _service = service;
            _estudianteService = estudianteService;
            _materiaService = materiaService;
        }

        // GET: Calificacions
        public async Task<IActionResult> Index()
        {
            var calificaciones = await _service.GetAllAsync();
            return View(calificaciones);
        }

        // GET: Calificacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var calificacion = await _service.GetByIdAsync(id.Value);
            if (calificacion == null) return NotFound();

            return View(calificacion);
        }

        // GET: Calificacions/Create
        public async Task<IActionResult> Create()
        {
            await CargarCombos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CalificacionCreateVM model)
        {
            if (ModelState.IsValid)
            {
                // Mapear a la entidad
                var calificacion = new Calificacion
                {
                    EstudianteId = model.EstudianteId,
                    MateriaId = model.MateriaId,
                    Calificacion1 = model.Calificacion1,
                    Calificacion2 = model.Calificacion2,
                    Calificacion3 = model.Calificacion3,
                    Calificacion4 = model.Calificacion4,
                    Examen = model.Examen,
                    Clasificacion = "Pendiente", // Valor por defecto
                    Estado = "Activo"             // Valor por defecto
                };

                await _service.AddAsync(calificacion);
                return RedirectToAction(nameof(Index));
            }

            await CargarCombos();
            return View(model);
        }

        // GET: Calificacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var calificacion = await _service.GetByIdAsync(id.Value);
            if (calificacion == null) return NotFound();

            // Mapear la entidad al ViewModel
            var model = new CalificacionCreateVM
            {
                EstudianteId = calificacion.EstudianteId,
                MateriaId = calificacion.MateriaId,
                Calificacion1 = calificacion.Calificacion1,
                Calificacion2 = calificacion.Calificacion2,
                Calificacion3 = calificacion.Calificacion3,
                Calificacion4 = calificacion.Calificacion4,
                Examen = calificacion.Examen
            };

            await CargarCombos(calificacion);
            return View(model);
        }

        // POST: Calificacions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CalificacionCreateVM model)
        {
            if (ModelState.IsValid)
            {

                await _service.UpdateAsync(id , model);
                return RedirectToAction(nameof(Index));
            }

            await CargarCombos();
            return View(model);
        }


        // GET: Calificacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var calificacion = await _service.GetByIdAsync(id.Value);
            if (calificacion == null) return NotFound();

            return View(calificacion);
        }

        // POST: Calificacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarCombos(Calificacion? calificacion = null)
        {
            var estudiantes = await _estudianteService.GetAllAsync();
            var materias = await _materiaService.GetAllMateriasAsync();

            // Concatenamos apellido + nombre
            var estudiantesCombo = estudiantes.Select(e => new
            {
                EstudianteId = e.EstudianteId,
                NombreCompleto = $"{e.Apellido} {e.Nombre}"
            });

            ViewData["EstudianteId"] = new SelectList(
                estudiantesCombo,
                "EstudianteId",
                "NombreCompleto",
                calificacion?.EstudianteId
            );

            ViewData["MateriaId"] = new SelectList(
                materias,
                "MateriaId",
                "Nombre",
                calificacion?.MateriaId
            );
        }

        [HttpGet]
        public async Task<IActionResult> ExportToCsv()
        {
            try
            {
                var csvBytes = await ToCsvData();
                return File(csvBytes, "text/csv", "Calificaciones.csv");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        private async Task<byte[]> ToCsvData()
        {
            var calificacionsData = await _service.ExportToCsvAsync();

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
