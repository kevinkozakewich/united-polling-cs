using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UnitedPolling.Services;
using UnitedPolling.DataContext.Models;
using UnitedPolling.DataContext;
using Microsoft.AspNetCore.Authorization;

namespace UnitedPolling.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly UnitedPollingDbContext _context;
        private readonly IPollService _pollService;
        private readonly IUserService _userService;

        public PollController(UnitedPollingDbContext context, IPollService pollServce, IUserService userService)
        {
            _context = context;
            _pollService = pollServce;
            _userService = userService;
        }

        [HttpGet]
        [Route("GetPolls")]
        public async Task<IActionResult> GetPolls()
        {
            var userId = await _userService.GetIdByAspUser(User);

            var results = await _pollService.GetUserPolls(userId);

            return Ok(results);
        }

        [HttpGet]
        [Route("GetPollQuestionList/{pollId}")]
        public async Task<IActionResult> GetPollQuestionList(int pollId)
        {
            var userId = await _userService.GetIdByAspUser(User);

            var results = await _pollService.GetPollQuestionList(pollId, userId);

            return Ok(results);
        }

        [HttpPost]
        [Route("CreatePoll")]
        public async Task<IActionResult> CreatePoll(CreatePollDto createPollDto)
        {
            var userId = await _userService.GetIdByAspUser(User);

            var pollCreated = await _pollService.CreatePoll(createPollDto, userId);

            return Ok(pollCreated);
        }
    }
}
