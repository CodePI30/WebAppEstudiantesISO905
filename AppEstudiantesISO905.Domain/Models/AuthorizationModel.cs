using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Domain.Models
{
    public class AuthorizationModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
