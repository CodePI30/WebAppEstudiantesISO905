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
    public class MateriasController : Controller
    {
        private readonly IMateriaService _materiaService;

        public MateriasController(IMateriaService materiaService)
        {
            _materiaService = materiaService;
        }

        // GET: Materias
        public async Task<IActionResult> Index()
        {
            try
            {
                var materias = await _materiaService.GetAllMateriasAsync();
                return View(materias);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(Array.Empty<Materia>());
            }
        }

        // GET: Materias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var materia = await _materiaService.GetMateriaByIdAsync(id.Value);
                return View(materia);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Materias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MateriaId,Nombre,Creditos")] Materia materia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _materiaService.CrearMateriaAsync(materia);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            return View(materia);
        }

        // GET: Materias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var materia = await _materiaService.GetMateriaByIdAsync(id.Value);
                return View(materia);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Materias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MateriaId,Nombre,Creditos")] Materia materia)
        {
            if (id != materia.MateriaId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _materiaService.ActualizarMateriaAsync(materia);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(materia);
                }
            }
            return View(materia);
        }

        // GET: Materias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var materia = await _materiaService.GetMateriaByIdAsync(id.Value);
                return View(materia);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Materias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _materiaService.EliminarMateriaAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
