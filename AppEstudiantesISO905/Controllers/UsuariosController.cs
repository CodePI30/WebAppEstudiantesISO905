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
using Microsoft.AspNetCore.Authorization;

namespace AppEstudiantesISO905.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _service.ObtenerUsuarios());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Usuario usuario = new Usuario();

            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                usuario = await _service.ObtenerUsuarioById(id);

                if (usuario == null)
                {
                    return NotFound();
                }
            }
            catch (ExceptionApp ex)
            {
                ViewData["ErrorMessage"] = ex.InnerException?.Message;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nombre,Email,PasswordHash,Estado")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.CrearUsuario(usuario);
                    return RedirectToAction(nameof(Index));
                }
                catch (ExceptionApp ex)
                {
                    // Captura la excepción personalizada y muestra mensaje en la vista
                    ViewData["ErrorMessage"] = ex.InnerException?.Message;
                }
                catch (Exception ex)
                {
                    // Para cualquier otra excepción no esperada
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }

            // Si hay errores de validación o excepciones, volvemos a la vista con el usuario ingresado
            return View(usuario);
        }


        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _service.ObtenerUsuarioForActions(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nombre,Email,PasswordHash,Estado")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.EditarUsuario(id, usuario);
                }
                catch (NotFoundException)
                {
                    Response.HttpContext.Response.StatusCode = 404;
                }
                catch (ExceptionApp ex)
                {
                    // Captura la excepción personalizada y muestra mensaje en la vista
                    ViewData["ErrorMessage"] = ex.InnerException?.Message;
                }
                catch (Exception ex)
                {
                    // Para cualquier otra excepción no esperada
                    ViewData["ErrorMessage"] = ex.Message;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _service.ObtenerUsuarioForActions(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.EliminarUsuario(id);

            }
            catch (ExceptionApp ex)
            {

                ViewData["ErrorMessage"] = ex.InnerException?.Message;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
