using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.Services.Exceptions
{
    public class InvalidProfileException : Exception
    {
        public InvalidProfileException()
        {
        }

        public InvalidProfileException(string message)
            : base(message)
        {
        }

        public InvalidProfileException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
