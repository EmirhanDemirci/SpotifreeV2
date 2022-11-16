using Spotifree.AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.AuthService.Logic.Interfaces
{
    public interface IAuthLogic
    {
        void CreateCredentials(Credentials credentials);
        CredentialsWithToken Authenticate(string email, string password);
        string HashPassword(string password);
        void DeHashPassword(string password, string dbPassword);
    }
}
