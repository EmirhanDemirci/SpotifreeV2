using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.DataAccess.Data.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        IProfileRepository Profile { get; }

        void Save();
    }
}
