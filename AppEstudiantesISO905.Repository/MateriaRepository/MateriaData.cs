using AppEstudiantesISO905.Domain.Contracts;
using AppEstudiantesISO905.Domain.ExceptionManager;
using AppEstudiantesISO905.Domain.Models;
using AppEstudiantesISO905.Repository.WebAppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Repository.MateriaRepository
{
    public class MateriaData : IMateriaData
    {
        private readonly EstudiantesIso905Context _context;

        public MateriaData(EstudiantesIso905Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Materia>> GetAllAsync()
        {
            try
            {
                return await _context.Materia.ToListAsync();
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Error al obtener las materias", ex);
            }
        }

        public async Task<Materia> GetByIdAsync(int id)
        {
            try
            {
                var materia = await _context.Materia.FirstOrDefaultAsync(m => m.MateriaId == id);

                if (materia == null)
                    throw new ExceptionApp("Materia no encontrada");

                return materia;
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp($"Error al obtener la materia con Id {id}", ex);
            }
        }

        public async Task CreateAsync(Materia materia)
        {
            try
            {
                _context.Add(materia);
                await _context.SaveChangesAsync();
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Error al crear la materia", ex);
            }
        }

        public async Task UpdateAsync(Materia materia)
        {
            try
            {
                _context.Update(materia);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(materia.MateriaId))
                    throw new ExceptionApp("La materia no existe");
                else
                    throw;
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Error al actualizar la materia", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var materia = await _context.Materia.FindAsync(id);
                if (materia == null)
                    throw new ExceptionApp("Materia no encontrada");

                _context.Materia.Remove(materia);
                await _context.SaveChangesAsync();
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Error al eliminar la materia", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.Materia.AnyAsync(e => e.MateriaId == id);
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Error al verificar existencia de la materia", ex);
            }
        }
    }
}
