using AppEstudiantesISO905.Domain.Contracts;
using AppEstudiantesISO905.Domain.ExceptionManager;
using AppEstudiantesISO905.Domain.Models;
using AppEstudiantesISO905.Repository.WebAppDbContext;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Repository.CalificacionesRepository
{
    public class CalificacionData : ICalificacionData
    {
        private readonly EstudiantesIso905Context _context;

        public CalificacionData(EstudiantesIso905Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Calificacion>> GetAllAsync()
        {
            try
            {
                return await _context.Calificacions
                .Include(c => c.Estudiante)
                .Include(c => c.Materia)
                .ToListAsync();
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Error al obtener las calificaciones", ex);
            }
        }

        public async Task<Calificacion?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Calificacions
                    .Include(c => c.Estudiante)
                    .Include(c => c.Materia)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Error al obtener las calificaciones por ID", ex);
            }
        }

        public async Task AddAsync(Calificacion calificacion)
        {
            try
            {
                await _context.Calificacions.AddAsync(calificacion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Error al agregar una nueva calificacion", ex);
            }
        }

        public async Task UpdateAsync(int id, CalificacionCreateVM calificacion)
        {
            try
            {
                var calificacionOrg = await GetByIdAsync(id);

                if (calificacionOrg is null)
                    throw new ExceptionApp("Error al obtener la calificación");

                calificacionOrg.EstudianteId = calificacion.EstudianteId;
                calificacionOrg.MateriaId = calificacion.MateriaId;
                calificacionOrg.Calificacion1 = calificacion.Calificacion1;
                calificacionOrg.Calificacion2 = calificacion.Calificacion2;
                calificacionOrg.Calificacion3 = calificacion.Calificacion3;
                calificacionOrg.Calificacion4 = calificacion.Calificacion4;
                calificacionOrg.Examen = calificacion.Examen;

                var promedio = (calificacion.Calificacion1 + calificacion.Calificacion2 +
                                calificacion.Calificacion3 + calificacion.Calificacion4) / 4m;

                var totalCalificacion = (promedio * 0.7m) + (calificacion.Examen * 0.3m);

                string clasificacion;
                if (totalCalificacion >= 90)
                    clasificacion = "A";
                else if (totalCalificacion >= 80)
                    clasificacion = "B";
                else if (totalCalificacion >= 70)
                    clasificacion = "C";
                else
                    clasificacion = "F";

                string estado = (totalCalificacion >= 70) ? "Aprobado" : "Reprobado";

                calificacionOrg.PromedioCalificaciones = promedio;
                calificacionOrg.TotalCalificacion = totalCalificacion;
                calificacionOrg.Clasificacion = clasificacion;
                calificacionOrg.Estado = estado;

                _context.Calificacions.Update(calificacionOrg);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al actualizar una calificación", ex);
            }
        }


        public async Task DeleteAsync(Calificacion calificacion)
        {
            try
            {
                _context.Calificacions.Remove(calificacion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Error al actualizar una calificacion", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Calificacions.AnyAsync(c => c.Id == id);
        }

        public async Task<List<Calificacion>> ExportToCsvAsync()
        {
            var calificaciones = await _context.Calificacions
                .Include(c => c.Estudiante)
                .Include(c => c.Materia)
                .ToListAsync();

            if (calificaciones == null || !calificaciones.Any())
                throw new Exception("No hay calificaciones para exportar.");

            return calificaciones;
        }
    }
}
