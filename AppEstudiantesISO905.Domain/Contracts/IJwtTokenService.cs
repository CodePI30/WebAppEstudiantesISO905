using AppEstudiantesISO905.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Domain.Contracts
{
    public interface IJwtTokenService
    {
        string GenerateToken(Usuario usuario);
    }
}
