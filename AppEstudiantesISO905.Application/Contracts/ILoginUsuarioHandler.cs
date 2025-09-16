using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Application.Contracts
{
    public interface ILoginUsuarioHandler
    {
        Task<string> Handle(string email, string password);
    }
}
