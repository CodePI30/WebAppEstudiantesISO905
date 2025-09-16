using AppEstudiantesISO905.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Domain.Contracts
{
    public interface IUsuarioData
    {
        Task<IEnumerable<Usuario>> ObtenerUsuarios(string? email = null);
        Task CrearUsuario(Usuario usuario);
        Task<Usuario> ObtenerUsuarioForActions(int id);
        Task EditarUsuario(Usuario usuario);
        Task EliminarUsuario(int id);
        Task<Usuario> ObtenerUsuarioById(int? id);
        Task<Usuario?> ObtenerUsuariosByEmail(string? email = null);
    }
}
