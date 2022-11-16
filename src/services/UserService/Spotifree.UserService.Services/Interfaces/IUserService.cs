using Spotifree.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.Services.Interfaces
{
    public interface IUserService
    {
        void Create(UserViewModel userViewModel);
        User Get(int id);
        void Delete(User user);
    }
}
