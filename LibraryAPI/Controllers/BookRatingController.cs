namespace LibraryAPI.Controllers
{
    using LibraryAPI.DTOs;
    using LibraryAPI.Service.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    public class BookRatingController : ControllerBase
    {
        private readonly IBookRatingService _ratingService;

        public BookRatingController(IBookRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // GET api/BookRating/{bookId} — Lấy tổng quan rating + rating của tôi
        // AllowAnonymous để guest xem được
        [HttpGet("{bookId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRatingSummary(int bookId)
        {
            // userId = null nếu guest
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.TryParse(userIdStr, out var id) ? id : 0;

            var result = await _ratingService.GetBookRatingSummaryAsync(userId, bookId);
            return Ok(result);
        }

        // POST api/BookRating — Lưu / cập nhật rating
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveRating([FromBody] SaveRatingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _ratingService.SaveRatingAsync(userId.Value, request);
            return Ok(result);
        }

        // DELETE api/BookRating/{bookId} — Xoá rating của tôi
        [HttpDelete("{bookId}")]
        [Authorize]
        public async Task<IActionResult> DeleteRating(int bookId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var deleted = await _ratingService.DeleteRatingAsync(userId.Value, bookId);
            if (!deleted)
                return NotFound(new { message = "Bạn chưa đánh giá sách này." });

            return Ok(new { message = "Đã xoá đánh giá." });
        }

        private int? GetUserId()
        {
            var str = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(str, out var id) ? id : null;
        }
    }
}