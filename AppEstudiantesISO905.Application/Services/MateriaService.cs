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
    public class MateriaService : IMateriaService
    {
        private readonly IMateriaData _repository;

        public MateriaService(IMateriaData repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Materia>> GetAllMateriasAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Error al obtener las materias", ex);
            }
        }

        public async Task<Materia> GetMateriaByIdAsync(int id)
        {
            try
            {
                var materia = await _repository.GetByIdAsync(id);
                if (materia == null)
                    throw new ExceptionApp($"No se encontró la materia con Id {id}");
                return materia;
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp($"Error al obtener la materia con Id {id}", ex);
            }
        }

        public async Task CrearMateriaAsync(Materia materia)
        {
            try
            {
                await _repository.CreateAsync(materia);
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Ha ocurrido un error al crear la materia", ex);
            }
        }

        public async Task ActualizarMateriaAsync(Materia materia)
        {
            try
            {
                await _repository.UpdateAsync(materia);
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Ha ocurrido un error al actualizar la materia", ex);
            }
        }

        public async Task EliminarMateriaAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Ha ocurrido un error al eliminar la materia", ex);
            }
        }

        public async Task<bool> MateriaExistsAsync(int id)
        {
            try
            {
                return await _repository.ExistsAsync(id);
            }
            catch (ExceptionApp ex)
            {
                throw new ExceptionApp("Error al verificar si la materia existe", ex);
            }
        }
    }
}
