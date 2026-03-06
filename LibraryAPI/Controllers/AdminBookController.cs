using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LibraryAPI.Controllers
{

    [Route("api/admin/books")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminBookController : ControllerBase
    {
        private readonly IAdminBookService _adminBookService;

        public AdminBookController(IAdminBookService adminBookService)
        {
            _adminBookService = adminBookService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var exists = await _adminBookService.BookExistsAsync(request.Title, request.Author);
            if (exists)
                return Conflict(new { message = $"Sách '{request.Title}' của '{request.Author}' đã tồn tại." });

            try
            {
                var result = await _adminBookService.CreateBookAsync(request);
                return CreatedAtAction(
                    nameof(GetBook),
                    new { bookId = result.BookId },
                    result
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBook(int bookId)
        {
            var book = await _adminBookService.GetBookByIdAsync(bookId);
            if (book == null)
                return NotFound(new { message = $"Không tìm thấy sách ID = {bookId}" });

            return Ok(book);
        }


        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] UpdateBookRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminBookService.UpdateBookAsync(bookId, request);
            if (result == null)
                return NotFound(new { message = $"Không tìm thấy sách ID = {bookId}" });

            return Ok(result);
        }


        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var deleted = await _adminBookService.DeleteBookAsync(bookId);
            if (!deleted)
                return NotFound(new { message = $"Không tìm thấy sách ID = {bookId}" });

            return Ok(new { message = $"Đã xoá sách ID = {bookId} thành công." });
        }
    }
}
