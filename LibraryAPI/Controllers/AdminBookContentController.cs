using LibraryAPI.Data;
using LibraryAPI.Data.Models;
using LibraryAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [Route("api/admin/books/{bookId}/contents")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminBookContentController : ControllerBase
    {
        private readonly PersonalLibraryContext _context;

        public AdminBookContentController(PersonalLibraryContext context)
        {
            _context = context;
        }

        // POST: api/admin/books/{bookId}/contents
        [HttpPost]
        public async Task<IActionResult> CreateChapter(int bookId, [FromBody] CreateChapterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return NotFound(new { message = $"Không tìm th?y sách ID = {bookId}" });

            var exists = await _context.BookContents
                .AnyAsync(c => c.BookId == bookId && c.ChapterNumber == request.ChapterNumber);
            if (exists)
                return Conflict(new { message = $"Ch??ng {request.ChapterNumber} ?ã t?n t?i cho sách ID = {bookId}" });

            var chapter = new BookContent
            {
                BookId = bookId,
                ChapterNumber = request.ChapterNumber,
                ChapterTitle = request.ChapterTitle?.Trim(),
                Content = request.Content,
                WordCount = CountWords(request.Content),
                CreatedAt = DateTime.Now
            };

            _context.BookContents.Add(chapter);
            await _context.SaveChangesAsync();

            var result = new
            {
                chapter.ContentId,
                BookId = chapter.BookId,
                chapter.ChapterNumber,
                chapter.ChapterTitle,
                chapter.WordCount
            };

            return CreatedAtAction(
                nameof(GetChapter),
                new { bookId = bookId, chapterNumber = chapter.ChapterNumber },
                result
            );
        }

        // GET: api/admin/books/{bookId}/contents
        [HttpGet]
        public async Task<IActionResult> GetChapters(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return NotFound(new { message = $"Không tìm th?y sách ID = {bookId}" });

            var chapters = await _context.BookContents
                .Where(c => c.BookId == bookId)
                .OrderBy(c => c.ChapterNumber)
                .Select(c => new
                {
                    c.ContentId,
                    c.ChapterNumber,
                    c.ChapterTitle,
                    c.WordCount,
                })
                .ToListAsync();

            return Ok(chapters);
        }

        // GET: api/admin/books/{bookId}/contents/{chapterNumber}
        [HttpGet("{chapterNumber}")]
        public async Task<IActionResult> GetChapter(int bookId, int chapterNumber)
        {
            var chapter = await _context.BookContents
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.ChapterNumber == chapterNumber);

            if (chapter == null)
                return NotFound(new { message = $"Không tìm th?y ch??ng {chapterNumber} cho sách ID = {bookId}" });

            var result = new
            {
                chapter.ContentId,
                BookId = chapter.BookId,
                chapter.ChapterNumber,
                chapter.ChapterTitle,
                chapter.Content,
                chapter.WordCount,
                chapter.CreatedAt
            };

            return Ok(result);
        }

        // PUT: api/admin/books/{bookId}/contents/{chapterNumber}
        [HttpPut("{chapterNumber}")]
        public async Task<IActionResult> UpdateChapter(int bookId, int chapterNumber, [FromBody] UpdateChapterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var chapter = await _context.BookContents
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.ChapterNumber == chapterNumber);

            if (chapter == null)
                return NotFound(new { message = $"Không tìm th?y ch??ng {chapterNumber} cho sách ID = {bookId}" });

            // If changing chapter number, ensure new number is not already used
            if (request.ChapterNumber.HasValue && request.ChapterNumber.Value != chapterNumber)
            {
                var conflict = await _context.BookContents
                    .AnyAsync(c => c.BookId == bookId && c.ChapterNumber == request.ChapterNumber.Value);
                if (conflict)
                    return Conflict(new { message = $"Ch??ng {request.ChapterNumber.Value} ?ã t?n t?i cho sách ID = {bookId}" });

                chapter.ChapterNumber = request.ChapterNumber.Value;
            }

            if (request.ChapterTitle != null) chapter.ChapterTitle = request.ChapterTitle.Trim();
            if (request.Content != null)
            {
                chapter.Content = request.Content;
                chapter.WordCount = CountWords(request.Content);
            }

            await _context.SaveChangesAsync();

            var result = new
            {
                chapter.ContentId,
                BookId = chapter.BookId,
                chapter.ChapterNumber,
                chapter.ChapterTitle,
                chapter.WordCount,
                chapter.CreatedAt
            };

            return Ok(result);
        }

        // DELETE: api/admin/books/{bookId}/contents/{chapterNumber}
        [HttpDelete("{chapterNumber}")]
        public async Task<IActionResult> DeleteChapter(int bookId, int chapterNumber)
        {
            var chapter = await _context.BookContents
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.ChapterNumber == chapterNumber);

            if (chapter == null)
                return NotFound(new { message = $"Không tìm th?y ch??ng {chapterNumber} cho sách ID = {bookId}" });

            _context.BookContents.Remove(chapter);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"?ã xoá ch??ng {chapterNumber} c?a sách ID = {bookId}." });
        }

        private static int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }

    public class UpdateChapterRequest
    {
        [Range(1, 9999)]
        public int? ChapterNumber { get; set; }

        [MaxLength(255)]
        public string? ChapterTitle { get; set; }

        public string? Content { get; set; }
    }
}
