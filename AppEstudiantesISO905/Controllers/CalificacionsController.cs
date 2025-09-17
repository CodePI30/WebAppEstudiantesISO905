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

        // POST: Calificacions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EstudianteId,MateriaId,Calificacion1,Calificacion2,Calificacion3,Calificacion4,Examen,PromedioCalificaciones,TotalCalificacion,Clasificacion,Estado")] Calificacion calificacion)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(calificacion);
                return RedirectToAction(nameof(Index));
            }
            await CargarCombos(calificacion);
            return View(calificacion);
        }

        // GET: Calificacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var calificacion = await _service.GetByIdAsync(id.Value);
            if (calificacion == null) return NotFound();

            await CargarCombos(calificacion);
            return View(calificacion);
        }

        // POST: Calificacions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EstudianteId,MateriaId,Calificacion1,Calificacion2,Calificacion3,Calificacion4,Examen,PromedioCalificaciones,TotalCalificacion,Clasificacion,Estado")] Calificacion calificacion)
        {
            if (id != calificacion.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(calificacion);
                return RedirectToAction(nameof(Index));
            }
            await CargarCombos(calificacion);
            return View(calificacion);
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

            ViewData["EstudianteId"] = new SelectList(estudiantes, "EstudianteId", "Apellido", calificacion?.EstudianteId);
            ViewData["MateriaId"] = new SelectList(materias, "MateriaId", "Nombre", calificacion?.MateriaId);
        }
    }
}
