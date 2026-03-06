
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

  
        [HttpGet("{id}/chapters")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChapters(int id)
            => Ok(await _bookService.GetChapterListAsync(id));

     
        [HttpGet("{id}/chapters/{chapterNumber}")]
        [Authorize] 
        public async Task<IActionResult> GetChapter(int id, int chapterNumber)
        {
            var result = await _bookService.GetChapterAsync(id, chapterNumber);

            
            if (result == null)
                return NoContent(); 

            return Ok(result);
        }
    }
}