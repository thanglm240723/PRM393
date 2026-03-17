namespace LibraryAPI.Controllers
{
    using LibraryAPI.Data;
    using LibraryAPI.Data.Models;
    using LibraryAPI.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly PersonalLibraryContext _context;

        public ProfileController(PersonalLibraryContext context)
        {
            _context = context;
        }

        // PUT api/Profile — Cập nhật thông tin cá nhân
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return NotFound();

            // Kiểm tra email trùng với user khác
            if (!string.IsNullOrEmpty(request.Email))
            {
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == request.Email && u.UserId != userId.Value);
                if (emailExists)
                    return Conflict(new { message = "Email đã được sử dụng bởi tài khoản khác." });
            }

            if (request.FullName != null) user.FullName = request.FullName.Trim();
            if (request.Email != null) user.Email = request.Email.Trim();
            if (request.AvatarUrl != null) user.AvatarUrl = request.AvatarUrl.Trim();
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật thành công.",
                fullName = user.FullName,
                email = user.Email,
                avatarUrl = user.AvatarUrl,
            });
        }

        // PUT api/Profile/change-password — Đổi mật khẩu
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return NotFound();

            // Kiểm tra mật khẩu hiện tại
            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                return BadRequest(new { message = "Mật khẩu hiện tại không đúng." });

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công." });
        }

        // POST api/Profile/track-time — Ghi lịch sử thời gian đọc
        // Flutter gọi khi user rời khỏi ReadingScreen
        [HttpPost("track-time")]
        public async Task<IActionResult> TrackReadingTime([FromBody] TrackReadingTimeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            // Ghi vào ReadingHistory
            _context.ReadingHistories.Add(new ReadingHistory
            {
                UserId = userId.Value,
                BookId = request.BookId,
                MinutesRead = request.MinutesRead,
                ReadAt = DateTime.Now,
            });

            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã ghi lịch sử đọc." });
        }

        // GET api/Profile/reading-history — Lịch sử đọc của user
        [HttpGet("reading-history")]
        public async Task<IActionResult> GetReadingHistory(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var query = _context.ReadingHistories
                .Where(h => h.UserId == userId.Value)
                .Include(h => h.Book)
                .OrderByDescending(h => h.ReadAt);

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(h => new
                {
                    historyId = h.HistoryId,
                    bookId = h.BookId,
                    bookTitle = h.Book != null ? h.Book.Title : "Unknown",
                    bookCover = h.Book != null ? h.Book.CoverImageUrl : null,
                    minutesRead = h.MinutesRead,
                    readAt = h.ReadAt,
                })
                .ToListAsync();

            return Ok(new
            {
                items,
                totalCount = total,
                pageNumber = page,
                pageSize,
            });
        }

        private int? GetUserId()
        {
            var str = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(str, out var id) ? id : null;
        }
    }
}