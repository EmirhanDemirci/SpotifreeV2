using Spotifree.UserService.DataAccess.Data.Repository.Interfaces;
using Spotifree.UserService.Models;
using Spotifree.UserService.Services.Exceptions;
using Spotifree.UserService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.Services
{

    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Delete(Profile profile)
        {
            throw new NotImplementedException();
        }

        public Profile Get(int id)
        {
            if (id == 0)
                return null;

            var profile = _unitOfWork.Profile.Get(id);
            return profile;
        }

        public void Upload(int id, byte[] picture)
        {
            if (id == 0)
                throw new InvalidProfileException("Invalid id given");

            var profile = _unitOfWork.Profile.Get(id);
            if (profile == null)
                throw new InvalidProfileException("Profile does not exist");

            profile.ProfilePicture = picture;
            _unitOfWork.Profile.Update(profile);
        }
    }
}
