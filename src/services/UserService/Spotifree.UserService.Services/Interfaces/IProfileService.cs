using Spotifree.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.Services.Interfaces
{
    public interface IProfileService
    {
        Profile Get(int id);
        void Delete(Profile profile);
        void Upload(int id, byte[] picture);
    }
}
