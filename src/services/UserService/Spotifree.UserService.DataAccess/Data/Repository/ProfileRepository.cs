using Spotifree.UserService.DataAccess.Data.Repository.Interfaces;
using Spotifree.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.DataAccess.Data.Repository
{
    public class ProfileRepository : Repository<Profile>, IProfileRepository
    {
        private readonly ApplicationDbContext _db;

        public ProfileRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Profile profile)
        {
            var objFromDb = _db.Profile.FirstOrDefault(s => s.Id == profile.Id);

            if (objFromDb != null)
            {
                objFromDb.Gender = profile.Gender;
                objFromDb.ProfilePicture = profile.ProfilePicture;
                objFromDb.Description = profile.Description;
            }

            _db.SaveChanges();
        }
    }
}
