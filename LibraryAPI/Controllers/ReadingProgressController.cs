using LibraryAPI.DTOs;
using LibraryAPI.Service;
using LibraryAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReadingProgressController : ControllerBase
    {
        private readonly IReadingProgressService _progressService;
        private readonly IGamificationService _gamification;

        public ReadingProgressController(
            IReadingProgressService progressService,
            IGamificationService gamification)
        {
            _progressService = progressService;
            _gamification = gamification;
        }

        // GET api/ReadingProgress/{bookId}
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetProgress(int bookId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _progressService.GetProgressAsync(userId.Value, bookId);

            if (result == null)
            {
                return Ok(new
                {
                    currentChapter = 1,
                    hasProgress = false,
                    progressPercentage = 0.0,
                });
            }

            return Ok(new
            {
                currentChapter = result.CurrentChapter,
                hasProgress = true,
                progressPercentage = result.ProgressPercentage,
            });
        }

        // POST api/ReadingProgress
        // Response bây giờ bao gồm GamificationResult để Flutter show popup
        [HttpPost]
        public async Task<IActionResult> SaveProgress([FromBody] SaveProgressRequest dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var progress = await _progressService.SaveProgressAsync(userId.Value, dto);

            // GamificationResult đã được tính bên trong SaveProgressAsync
            // Lấy lại để trả về cho Flutter
            var gamResult = await _gamification.ProcessReadingProgressAsync(
                userId: userId.Value,
                bookId: dto.BookId,
                progressPercentage: progress.ProgressPercentage ?? 0,
                bookGenre: null, // đã xử lý trong service
                bookPageCount: null);

            return Ok(new
            {
                progress = progress,
                gamification = gamResult,
            });
        }

        // GET api/ReadingProgress
        [HttpGet]
        public async Task<IActionResult> GetAllProgress()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _progressService.GetAllProgressAsync(userId.Value);
            return Ok(result);
        }

        private int? GetUserId()
        {
            var str = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(str, out var id) ? id : null;
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> GetHistory()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Không tìm thấy thông tin người dùng.");

            var userId = int.Parse(userIdClaim);
            var history = await _progressService.GetReadingHistoryAsync(userId);
            return Ok(history);
        }

        [HttpGet("bookmarks")]
        [Authorize]
        public async Task<IActionResult> GetBookmarks()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Không tìm thấy thông tin người dùng.");

            var userId = int.Parse(userIdClaim);
            var bookmarks = await _progressService.GetBookmarksAsync(userId);
            return Ok(bookmarks);
        }
    }
}