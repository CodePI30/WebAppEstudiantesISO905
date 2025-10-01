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

namespace AppEstudiantesISO905.Repository.EstudianteRepository
{
    public class EstudianteData : IEstudianteData
    {
        private readonly EstudiantesIso905Context _context;

        public EstudianteData(EstudiantesIso905Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estudiante>> GetAllAsync()
        {
            try
            {
                return await _context.Estudiantes.ToListAsync();
            }
            catch (ExceptionApp ex)
            {

                throw new ExceptionApp("Error al obtener Estudiantes", ex);
            }
        }

        public async Task<Estudiante?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Estudiantes.FirstOrDefaultAsync(e => e.EstudianteId == id);
            }
            catch (ExceptionApp ex)
            {

                throw new ExceptionApp("Error al obtener Estudiantes por ID", ex);
            }
        }

        public async Task AddAsync(Estudiante estudiante)
        {
            try
            {
                await _context.Estudiantes.AddAsync(estudiante);
                await _context.SaveChangesAsync();
            }
            catch (ExceptionApp ex)
            {

                throw new ExceptionApp("Error al guardar estudiante", ex);
            }
        }

        public async Task UpdateAsync(Estudiante estudiante)
        {
            try
            {
                _context.Estudiantes.Update(estudiante);
                await _context.SaveChangesAsync();
            }
            catch (ExceptionApp ex)
            {

                throw new ExceptionApp("Error al actualizar estudiante", ex);
            }
        }

        public async Task<Estudiante> GetLastSequence()
        {
            // Año actual
            string year = DateTime.Now.Year.ToString();

            // Buscar la última matrícula registrada de este año
            var ultimoEstudiante = _context.Estudiantes
                .Where(e => e.Matricula.StartsWith(year))
                .OrderByDescending(e => e.Matricula)
                .FirstOrDefault();

            int secuencia = 1;

            if (ultimoEstudiante != null)
            {
                // Tomamos los últimos 3 dígitos y sumamos 1
                string ultimoNumero = ultimoEstudiante.Matricula.Substring(4);
                secuencia = int.Parse(ultimoNumero) + 1;
            }

            // Generamos la matrícula con 3 dígitos
            string nuevaMatricula = $"{year}{secuencia:D3}";

            var estudiante = new Estudiante
            {
                Matricula = nuevaMatricula
            };

            return estudiante;
        }

        public async Task DeleteAsync(Estudiante estudiante)
        {
            try
            {
                string year = DateTime.Now.Year.ToString();
                string yearCreated = estudiante.Matricula.Substring(0, 4);

                if (year == yearCreated)
                    throw new ExceptionApp("No puedes eliminar un estudiante del ano en curso");

                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync();
            }
            catch (ExceptionApp ex)
            {

                throw new ExceptionApp("Error al eliminar estudiante", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Estudiantes.AnyAsync(e => e.EstudianteId == id);
        }

    }
}
