namespace LibraryAPI.Controllers
{
    using LibraryAPI.DTOs;
    using LibraryAPI.Service.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuoteController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        // POST api/Quote — Lưu quote mới
        [HttpPost]
        public async Task<IActionResult> SaveQuote([FromBody] SaveQuoteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _quoteService.SaveQuoteAsync(userId.Value, request);
            return CreatedAtAction(nameof(GetMyQuotes), null, result);
        }

        // GET api/Quote — Tất cả quotes của tôi
        [HttpGet]
        public async Task<IActionResult> GetMyQuotes([FromQuery] int? bookId = null)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _quoteService.GetMyQuotesAsync(userId.Value, bookId);
            return Ok(result);
        }

        // DELETE api/Quote/{quoteId}
        [HttpDelete("{quoteId}")]
        public async Task<IActionResult> DeleteQuote(int quoteId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var deleted = await _quoteService.DeleteQuoteAsync(userId.Value, quoteId);
            if (!deleted)
                return NotFound(new { message = "Không tìm thấy quote." });

            return Ok(new { message = "Đã xoá quote." });
        }

        private int? GetUserId()
        {
            var str = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(str, out var id) ? id : null;
        }
    }
}