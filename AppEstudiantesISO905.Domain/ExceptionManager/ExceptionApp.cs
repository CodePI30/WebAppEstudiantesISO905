using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEstudiantesISO905.Domain.ExceptionManager
{
    public class ExceptionApp : Exception
    {
        public ExceptionApp(string message)
            : base(message)
        {

        }

        public ExceptionApp(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}
