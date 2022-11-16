using Microsoft.AspNetCore.Mvc;
using Spotifree.AuthService.Logic.Interfaces;
using Spotifree.AuthService.Models;

namespace Spotifree.AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthLogic _authLogic;
        public AuthController(ILogger<AuthController> logger, IAuthLogic authLogic)
        {
            _logger = logger;
            _authLogic = authLogic;
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            CredentialsWithToken userWithToken;
            try
            {
                userWithToken = _authLogic.Authenticate(model.Email, model.Password);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok(userWithToken);
        }
        [HttpPost("create")]
        public IActionResult Create(Credentials credentials)
        {
            try
            {
                _authLogic.CreateCredentials(credentials);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }
    }
}
