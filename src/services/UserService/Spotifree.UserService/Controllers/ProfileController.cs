using Microsoft.AspNetCore.Mvc;
using Spotifree.UserService.DataAccess.Data.Repository.Interfaces;
using Spotifree.UserService.Models;
using Spotifree.UserService.Services.Interfaces;

namespace Spotifree.UserService.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IProfileService _profileService;

        public ProfileController(ILogger<UserController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _profileService = new Services.ProfileService(unitOfWork);
        }
        [HttpGet("{id}")]
        public Profile Get([FromRoute] int id)
        {
            return _profileService.Get(id);
        }

        [HttpPost("upload/{id}")]
        public async Task<IActionResult> Upload([FromRoute] int id, IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    try
                    {
                        _profileService.Upload(id, memoryStream.ToArray());
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new { message = e.Message });
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                    return BadRequest();
                }
            }
            return Ok();
        }
    }
}
