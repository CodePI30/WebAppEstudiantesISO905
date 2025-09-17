using AppEstudiantesISO905.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Application.Contracts
{
    public interface IMateriaService
    {
        Task<IEnumerable<Materia>> GetAllMateriasAsync();
        Task<Materia> GetMateriaByIdAsync(int id);
        Task CrearMateriaAsync(Materia materia);
        Task ActualizarMateriaAsync(Materia materia);
        Task EliminarMateriaAsync(int id);
        Task<bool> MateriaExistsAsync(int id);
    }
}
