using Microsoft.AspNetCore.Mvc;
using Spotifree.UserService.DataAccess.Data.Repository.Interfaces;
using Spotifree.UserService.Models;
using Spotifree.UserService.Services.Interfaces;

namespace Spotifree.UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUnitOfWork unitOfWork, IRabbitManager manager)
        {
            _logger = logger;
            _userService = new Services.UserService(unitOfWork, manager);
        }

        [HttpPost("create")]
        public IActionResult Create(UserViewModel userViewModel)
        {
            try
            {
                _userService.Create(userViewModel);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return Ok(_userService.Get(id));
        }
    }
}
