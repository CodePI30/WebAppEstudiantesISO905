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

        public async Task UpdateAsync(Calificacion calificacion)
        {
            try
            {
                _context.Calificacions.Update(calificacion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Error al actualizar una calificacion", ex);
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

    }
}
