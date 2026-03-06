using LibraryAPI.DTOs;
using LibraryAPI.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LibraryAPI.Service.Interface;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReadingProgressController : ControllerBase
    {
        private readonly IReadingProgressService _progressService;
        private readonly PersonalLibraryContext _context;

        public ReadingProgressController(
            IReadingProgressService progressService,
            PersonalLibraryContext context)
        {
            _progressService = progressService;
            _context = context;
        }

   
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetProgress(int bookId)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdStr == null) return Unauthorized();
            var userId = int.Parse(userIdStr);

            var result = await _progressService.GetProgressAsync(userId, bookId);

            if (result == null)
            {
              
                return Ok(new
                {
                    currentChapter = 1,
                    hasProgress = false,
                    progressPercentage = 0.0
                });
            }

            return Ok(new
            {
                currentChapter = result.CurrentChapter,
                hasProgress = true,
                progressPercentage = result.ProgressPercentage
            });
        }

       
        [HttpPost]
        public async Task<IActionResult> SaveProgress([FromBody] SaveProgressRequest dto)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdStr == null) return Unauthorized();
            var userId = int.Parse(userIdStr);

            var totalChapters = await _context.BookContents
                .CountAsync(c => c.BookId == dto.BookId);

            var result = await _progressService.SaveProgressAsync(userId, dto, totalChapters);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProgress()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdStr == null) return Unauthorized();
            var userId = int.Parse(userIdStr);

            var result = await _progressService.GetAllProgressAsync(userId);
            return Ok(result);
        }
    }
}