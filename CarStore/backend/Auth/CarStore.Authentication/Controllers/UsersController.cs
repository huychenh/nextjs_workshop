using CarStore.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarStore.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("get-user-email")]
        public async Task<ActionResult> GetUserEmail(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Ok(null);
            }

            var user = await _userManager.FindByIdAsync(userId);
            return Ok(user?.Email);
        }
    }
}
