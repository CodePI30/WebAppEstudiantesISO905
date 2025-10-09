using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Domain.Contracts;
using AppEstudiantesISO905.Domain.ExceptionManager;
using AppEstudiantesISO905.Domain.Models;
using DocumentFormat.OpenXml.InkML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Application.Services
{
    public class CalificacionService : ICalificacionService
    {
        private readonly ICalificacionData _repository;

        public CalificacionService(ICalificacionData repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Calificacion>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al obtener las calificaciones.", ex);
            }
        }

        public async Task<Calificacion?> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp($"Error al obtener la calificación con ID {id}.", ex);
            }
        }

        public async Task AddAsync(Calificacion calificacion)
        {
            try
            {
                ValidarNotas(calificacion);
                await _repository.AddAsync(calificacion);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al crear la calificación.", ex);
            }
        }

        public async Task UpdateAsync(int id ,CalificacionCreateVM calificacion)
        {
            try
            {
                if (!await _repository.ExistsAsync(id))
                    throw new ExceptionApp($"La calificación con ID {id} no existe.");

                ValidarNotas(calificacion);
                await _repository.UpdateAsync(id, calificacion);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al actualizar la calificación.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var calificacion = await _repository.GetByIdAsync(id);
                if (calificacion == null)
                    throw new ExceptionApp($"La calificación con ID {id} no existe.");

                await _repository.DeleteAsync(calificacion);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Error al eliminar la calificación.", ex);
            }
        }

        private void ValidarNotas(Calificacion calificacion)
        {
            var notas = new[]
            {
                calificacion.Calificacion1,
                calificacion.Calificacion2,
                calificacion.Calificacion3,
                calificacion.Calificacion4,
                calificacion.Examen
            };

            if (notas.Any(n => n < 0 || n > 100))
                throw new ExceptionApp("Todas las calificaciones deben estar en el rango de 0 a 100.");

            if (calificacion.PromedioCalificaciones < 0 || calificacion.PromedioCalificaciones > 100)
                throw new ExceptionApp("El promedio debe estar en el rango de 0 a 100.");
        }


        public async Task<byte[]> ExportToCsvAsync()
        {
            try
            {
                return await _repository.ExportToCsvAsync();
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Error al exportar la data", ex);
            }
        }

        private void ValidarNotas(CalificacionCreateVM calificacion)
        {
            var notas = new[]
            {
                calificacion.Calificacion1,
                calificacion.Calificacion2,
                calificacion.Calificacion3,
                calificacion.Calificacion4,
                calificacion.Examen
            };

            if (notas.Any(n => n < 0 || n > 100))
                throw new ExceptionApp("Todas las calificaciones deben estar en el rango de 0 a 100.");
        }
    }
}
