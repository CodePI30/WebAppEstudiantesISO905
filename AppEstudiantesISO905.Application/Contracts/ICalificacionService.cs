using AppEstudiantesISO905.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Application.Contracts
{
    public interface ICalificacionService
    {
        Task<IEnumerable<Calificacion>> GetAllAsync();
        Task<Calificacion?> GetByIdAsync(int id);
        Task AddAsync(Calificacion calificacion);
        Task UpdateAsync(Calificacion calificacion);
        Task DeleteAsync(int id);
    }
}
