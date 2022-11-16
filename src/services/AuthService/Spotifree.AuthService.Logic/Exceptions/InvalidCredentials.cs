using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.AuthService.Logic.Exceptions
{
    public  class InvalidCredentials : Exception
    {
        public InvalidCredentials()
        {

        }
        public InvalidCredentials(string message) : base(message)
        {
        }
        public InvalidCredentials(string message, Exception inner) : base(message, inner)
        {
        }
    }

}
