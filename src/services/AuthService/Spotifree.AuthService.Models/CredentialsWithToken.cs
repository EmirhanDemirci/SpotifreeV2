using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.AuthService.Models
{
    public class CredentialsWithToken
    {
        public Credentials Credentials { get; set; }
        public string Token { get; set; }
    }
}
