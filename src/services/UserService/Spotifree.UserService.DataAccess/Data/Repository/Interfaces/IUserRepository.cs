using Spotifree.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.DataAccess.Data.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User user);
        User GetWithProfile(int id);
    }
}
