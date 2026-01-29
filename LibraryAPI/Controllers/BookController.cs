using LibraryAPI.Service;
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

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books?page=1&pageSize=10
        [HttpGet]
        [AllowAnonymous] // Cho phép ai cũng xem được sách (hoặc xóa dòng này nếu muốn bắt buộc đăng nhập)
        public async Task<IActionResult> GetBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _bookService.GetBooksAsync(page, pageSize);
            return Ok(result);
        }
    }
}