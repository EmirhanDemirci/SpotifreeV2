using Spotifree.AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.AuthService.DataAccess.Data.Services.Interfaces
{
    public interface IAuthService
    {
        Credentials GetUser(string email);
        void Create(Credentials credentials);
    }
}
