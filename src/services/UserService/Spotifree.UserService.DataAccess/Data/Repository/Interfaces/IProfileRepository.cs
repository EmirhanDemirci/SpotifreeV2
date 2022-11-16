﻿using Spotifree.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.DataAccess.Data.Repository.Interfaces
{
    public interface IProfileRepository : IRepository<Profile>
    {
        void Update(Profile profile);
    }
}