// ── Service/BookService.cs ─────────────────────────────────────────
using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Service
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly PersonalLibraryContext _context;

        public BookService(IMapper mapper, IConfiguration configuration, PersonalLibraryContext context)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }

        public async Task<PagedResult<BookResponse>> GetBooksAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            var query = _context.Books.AsQueryable();
            var totalCount = await query.CountAsync();
            var books = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<BookResponse>
            {
                Items = _mapper.Map<List<BookResponse>>(books),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<BookResponse>> SearchBooksAsync(BookSearchRequest request)
        {
            var query = _context.Books.AsQueryable();
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(b => b.Title.Contains(term) || b.Author.Contains(term));
            }
            if (!string.IsNullOrWhiteSpace(request.Genre))
            {
                var term = request.Genre.Trim().ToLower();
                query = query.Where(b => b.Genre.ToLower().Contains(term));
            }
            var totalCount = await query.CountAsync();
            var books = await query.OrderByDescending(b => b.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();
            return new PagedResult<BookResponse>
            {
                Items = _mapper.Map<List<BookResponse>>(books),
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<BookDetailResponse?> GetBookByIdAsync(int bookId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
            if (book == null) return null;
            var totalChapters = await _context.BookContents.CountAsync(c => c.BookId == bookId);
            var response = _mapper.Map<BookDetailResponse>(book);
            response.TotalChapters = totalChapters;
            return response;
        }

        // Lấy danh sách chương (chỉ title, không có content để nhẹ)
        public async Task<List<ChapterListItem>> GetChapterListAsync(int bookId)
        {
            return await _context.BookContents
                .Where(c => c.BookId == bookId)
                .OrderBy(c => c.ChapterNumber)
                .Select(c => new ChapterListItem
                {
                    ContentId = c.ContentId,
                    ChapterNumber = c.ChapterNumber,
                    ChapterTitle = c.ChapterTitle,
                    WordCount = c.WordCount
                })
                .ToListAsync();
        }

        // Lấy nội dung 1 chương cụ thể
        public async Task<ChapterResponse?> GetChapterAsync(int bookId, int chapterNumber)
        {
            var chapter = await _context.BookContents
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.ChapterNumber == chapterNumber);
            if (chapter == null) return null;
            return new ChapterResponse
            {
                ContentId = chapter.ContentId,
                BookId = chapter.BookId ?? 0,
                ChapterNumber = chapter.ChapterNumber,
                ChapterTitle = chapter.ChapterTitle,
                Content = chapter.Content,
                WordCount = chapter.WordCount
            };
        }
    }
}