using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Domain.Contracts;
using AppEstudiantesISO905.Domain.ExceptionManager;
using AppEstudiantesISO905.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Application.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly IEstudianteData _repository;

        public EstudianteService(IEstudianteData repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Estudiante>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al obtener los estudiantes.", ex);
            }
        }

        public async Task<Estudiante?> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp($"Error al obtener el estudiante con ID {id}.", ex);
            }
        }

        public async Task AddAsync(Estudiante estudiante)
        {
            try
            {
                await _repository.AddAsync(estudiante);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al crear el estudiante.", ex);
            }
        }

        public async Task UpdateAsync(Estudiante estudiante)
        {
            try
            {
                if (!await _repository.ExistsAsync(estudiante.EstudianteId))
                    throw new ExceptionApp($"El estudiante con ID {estudiante.EstudianteId} no existe.");

                await _repository.UpdateAsync(estudiante);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al actualizar el estudiante.", ex);
            }
        }

        public async Task<Estudiante> GetLastSequence()
        {
            try
            {
                var estudianteLastSequence = await _repository.GetLastSequence();
                if (estudianteLastSequence == null)
                    throw new ExceptionApp($"Error al obtener secuencia");

                return estudianteLastSequence;
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al obtener secuencia.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var estudiante = await _repository.GetByIdAsync(id);
                if (estudiante == null)
                    throw new ExceptionApp($"El estudiante con ID {id} no existe.");

                await _repository.DeleteAsync(estudiante);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al eliminar el estudiante.", ex);
            }
        }
    }
}
