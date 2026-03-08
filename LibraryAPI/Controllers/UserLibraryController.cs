using LibraryAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bắt buộc user phải đăng nhập mới được gọi API này
    public class UserLibraryController : ControllerBase
    {
        private readonly IUserLibraryService _libraryService;

        public UserLibraryController(IUserLibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpPost("{bookId}/toggle-save")]
        public async Task<IActionResult> ToggleSave(int bookId)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var isSaved = await _libraryService.ToggleFavoriteAsync(userId, bookId);
            return Ok(new { isSaved = isSaved });
        }

        [HttpGet("saved")]
        public async Task<IActionResult> GetSavedBooks()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var books = await _libraryService.GetSavedBooksAsync(userId);
            return Ok(books);
        }

        [HttpGet("{bookId}/check-saved")]
        public async Task<IActionResult> CheckIsSaved(int bookId)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var isSaved = await _libraryService.CheckIsSavedAsync(userId, bookId);
            return Ok(new { isSaved = isSaved });
        }
    }
}