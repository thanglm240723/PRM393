// ── Controllers/BooksController.cs ───────────────────────────────────
using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService) => _bookService = bookService;

        // ── Public endpoints (không cần login) ───────────────────────

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBooks(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
            => Ok(await _bookService.GetBooksAsync(page, pageSize));

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchBooks([FromQuery] BookSearchRequest request)
            => Ok(await _bookService.SearchBooksAsync(request));

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookById(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        // Danh sách chương: public (để hiện số chương trên màn detail)
        [HttpGet("{id}/chapters")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChapters(int id)
            => Ok(await _bookService.GetChapterListAsync(id));

        // ── Nội dung chương: YÊU CẦU ĐĂNG NHẬP ─────────────────────
        [HttpGet("{id}/chapters/{chapterNumber}")]
        [Authorize]  // ← chặn người chưa login
        public async Task<IActionResult> GetChapter(int id, int chapterNumber)
        {
            var result = await _bookService.GetChapterAsync(id, chapterNumber);

            // Trả về null-safe: nếu chương không tồn tại → 204 NoContent
            // Flutter sẽ xử lý 204 như "hết chương" thay vì lỗi
            if (result == null)
                return NoContent(); // 204 — hết chương, không có lỗi

            return Ok(result);
        }
    }
}