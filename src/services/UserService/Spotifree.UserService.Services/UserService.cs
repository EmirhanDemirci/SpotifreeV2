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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IRabbitManager _manager;
        public UserService(IUnitOfWork unitOfWork, IRabbitManager manager)
        {
            _unitOfWork = unitOfWork;
            _manager = manager;
        }
        public void Create(UserViewModel userViewModel)
        {
            if (userViewModel.Email == null || userViewModel.Password == null || userViewModel.FirstName == null ||
                userViewModel.LastName == null ||
                userViewModel.Birthdate == null) throw new InvalidUserException("Not all properties are filled");

            var user = new User
            {
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                Birthdate = userViewModel.Birthdate
            };

            _unitOfWork.User.Add(user);
            _unitOfWork.Save();

            var credentials = new Credentials
            {
                Email = userViewModel.Email,
                Password = userViewModel.Password,
                UserId = user.Id
            };

            //var serializedObject = JsonSerializer.Serialize(credentials);

            _manager.Publish(new
            {
                credentials
            }, "exchange.topic.user.create", "topic", "*.queue.user.create.#");
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public User Get(int id)
        {
            if (id == 0)
                return null;

            var user = _unitOfWork.User.GetUser(id);

            if (user == null)
                return null;

            return user;
        }
    }
}
