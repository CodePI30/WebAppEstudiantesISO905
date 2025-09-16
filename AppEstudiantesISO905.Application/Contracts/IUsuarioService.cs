using AppEstudiantesISO905.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Application.Contracts
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ObtenerUsuarios(string? email = null);
        Task CrearUsuario(Usuario usuario);
        Task<Usuario> ObtenerUsuarioForActions(int id);
        Task EditarUsuario(int id, Usuario usuario);
        Task EliminarUsuario(int id);
        Task<Usuario> ObtenerUsuarioById(int? id);
    }
}
