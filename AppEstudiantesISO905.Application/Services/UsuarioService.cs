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
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioData _usuarioData;
        public UsuarioService(IUsuarioData usuarioData)
        {
            _usuarioData = usuarioData;
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuarios(string? email = null)
        {
            try
            {
                return await _usuarioData.ObtenerUsuarios(email);
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Ha ocurrido un error al intentar obtener los usuarios", ex);
            }
        }

        public async Task CrearUsuario(Usuario usuario)
        {
            try
            {
                await _usuarioData.CrearUsuario(usuario);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Ha ocurrido un error al intentar crear el usuario", ex);
            }
        }

        public async Task<Usuario> ObtenerUsuarioForActions(int id)
        {
            try
            {
                return await _usuarioData.ObtenerUsuarioForActions(id);
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Ha ocurrido un error al intentar consultar el usuario indicado", ex);
            }
        }

        public async Task EditarUsuario(int id, Usuario usuario)
        {
            try
            {
                if (id != usuario.UsuarioId)
                {
                    throw new ExceptionApp("El usuario indicado no coincide con el usuario a editar");
                }

                await _usuarioData.EditarUsuario(usuario);
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Ha ocurrido un error al intentar consultar el usuario indicado", ex);
            }
        }

        public async Task EliminarUsuario(int id)
        {
            try
            {
                await _usuarioData.EliminarUsuario(id);
            }
            catch (Exception ex)
            {
                throw new ExceptionApp("Ha ocurrido un error al intentar consultar el usuario indicado", ex);
            }
        }

        public async Task<Usuario> ObtenerUsuarioById(int? id)
        {
            try
            {
                return await _usuarioData.ObtenerUsuarioById(id);
            }
            catch (Exception ex)
            {

                throw new ExceptionApp("Ha ocurrido un error al intentar consultar el usuario indicado", ex);
            }
        }
    }
}
