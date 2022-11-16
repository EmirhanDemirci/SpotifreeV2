using Spotifree.AuthService.DataAccess.Data.Services.Interfaces;
using Spotifree.AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.AuthService.DataAccess.Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Credentials credentials)
        {
            _dbContext.Credentials.Add(credentials);
            _dbContext.SaveChanges();
        }

        public Credentials GetUser(string email)
        {
            return _dbContext.Credentials.FirstOrDefault(c => c.Email == email);
        }
    }
}
