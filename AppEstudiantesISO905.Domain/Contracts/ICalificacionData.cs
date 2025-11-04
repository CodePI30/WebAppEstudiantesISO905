using AppEstudiantesISO905.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Domain.Contracts
{
    public interface ICalificacionData
    {
        Task<IEnumerable<Calificacion>> GetAllAsync();
        Task<Calificacion?> GetByIdAsync(int id);
        Task AddAsync(Calificacion calificacion);
        Task UpdateAsync(int id, CalificacionCreateVM calificacion);
        Task DeleteAsync(Calificacion calificacion);
        Task<bool> ExistsAsync(int id);
        Task<List<Calificacion>> ExportToCsvAsync();
    }
}
