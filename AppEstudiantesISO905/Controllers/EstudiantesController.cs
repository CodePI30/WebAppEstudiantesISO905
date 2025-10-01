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
using AppEstudiantesISO905.Domain.ExceptionManager;

namespace AppEstudiantesISO905.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly IEstudianteService _service;

        public EstudiantesController(IEstudianteService service)
        {
            _service = service;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index()
        {
            var estudiantes = await _service.GetAllAsync();
            return View(estudiantes);
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var estudiante = await _service.GetByIdAsync(id.Value);
            if (estudiante == null) return NotFound();

            return View(estudiante);
        }

        // GET: Estudiantes/Create
        public async Task<IActionResult> Create()
        {
            var estudianteLastSequence = await _service.GetLastSequence();
            return View(estudianteLastSequence);
        }

        // POST: Estudiantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstudianteId,Nombre,Apellido,FechaNacimiento,Matricula")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                var estudianteLastSequence = await _service.GetLastSequence();

                estudiante.Matricula = estudianteLastSequence.Matricula;

                await _service.AddAsync(estudiante);
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var estudiante = await _service.GetByIdAsync(id.Value);
            if (estudiante == null) return NotFound();

            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EstudianteId,Nombre,Apellido,FechaNacimiento,Matricula")] Estudiante estudiante)
        {
            if (id != estudiante.EstudianteId) return NotFound();

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(estudiante);
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var estudiante = await _service.GetByIdAsync(id.Value);
            if (estudiante == null) return NotFound();

            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));

            }
            catch (ExceptionApp ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                var estudiante = await _service.GetByIdAsync(id);
                return View("Delete", estudiante);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar el estudiante.";
                var estudiante = await _service.GetByIdAsync(id);
                return View("Delete", estudiante);
            }
        }
    }
}
