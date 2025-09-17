using AppEstudiantesISO905.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Domain.Contracts
{
    public interface IMateriaData
    {
        Task<IEnumerable<Materia>> GetAllAsync();
        Task<Materia> GetByIdAsync(int id);
        Task CreateAsync(Materia materia);
        Task UpdateAsync(Materia materia);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
