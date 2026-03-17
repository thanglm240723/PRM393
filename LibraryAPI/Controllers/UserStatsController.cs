using LibraryAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserStatsController : ControllerBase
    {
        private readonly IGamificationService _gamification;

        public UserStatsController(IGamificationService gamification)
        {
            _gamification = gamification;
        }

       
        [HttpGet("me")]
        public async Task<IActionResult> GetMyStats()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var stats = await _gamification.GetUserStatsAsync(userId.Value);
            return Ok(stats);
        }

       
        [HttpGet("me/badges")]
        public async Task<IActionResult> GetMyBadges()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var badges = await _gamification.GetUserBadgesAsync(userId.Value);
            return Ok(badges);
        }

        
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard(
            [FromQuery] string type = "books",
            [FromQuery] int top = 20)
        {
           
            var validTypes = new[] { "books", "streak", "pages", "hours" };
            if (!validTypes.Contains(type))
                return BadRequest(new { message = "type phải là: books | streak | pages | hours" });

            if (top < 1 || top > 100) top = 20;

            var userId = GetUserId(); 
            var result = await _gamification.GetLeaderboardAsync(type, userId, top);
            return Ok(result);
        }

       
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserStats(int userId)
        {
            var stats = await _gamification.GetUserStatsAsync(userId);
         
            stats.LastReadDate = null;
            return Ok(stats);
        }

        private int? GetUserId()
        {
            var str = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(str, out var id) ? id : null;
        }
    }
}