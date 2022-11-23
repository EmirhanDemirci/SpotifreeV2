using Microsoft.EntityFrameworkCore;
using Spotifree.UserService.DataAccess.Data.Repository.Interfaces;
using Spotifree.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.DataAccess.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(User user)
        {
            var objFromDb = _db.User.FirstOrDefault(s => s.Id == user.Id);

            if (objFromDb != null)
            {
                objFromDb.FirstName = user.FirstName;
                objFromDb.LastName = user.LastName;
                objFromDb.Birthdate = user.Birthdate;
            }

            _db.SaveChanges();
        }
        public User GetUser(int id)
        {
            return _db.User.Where(u => u.Id == id).FirstOrDefault();
        }

        //public User GetWithProfile(int id)
        //{
        //    return _db.User
        //        .Where(u => u.Id == id)
        //        .Include(u => u.Profile)
        //        .FirstOrDefault();
        //}
    }
}
