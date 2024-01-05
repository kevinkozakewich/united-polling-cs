using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UnitedPolling.Services;
using UnitedPolling.DataContext;
using Microsoft.AspNetCore.Authorization;

namespace UnitedPolling.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UnitedPollingDbContext _context;
        private readonly IUserService _userService;
        
        public UserController(UnitedPollingDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        [Route("getLoggedInUser")]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var userId = await _userService.GetIdByAspUser(User);

            var user = await _userService.GetUserData(userId);

            return Ok(user);
        }
    }
}
