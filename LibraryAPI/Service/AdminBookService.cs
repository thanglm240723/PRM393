namespace LibraryAPI.Service
{
    using LibraryAPI.Data;
    using LibraryAPI.Data.Models;
    using LibraryAPI.DTOs;
    using LibraryAPI.Service.Interface;
    using Microsoft.EntityFrameworkCore;

    public class AdminBookService : IAdminBookService
    {
        private readonly PersonalLibraryContext _context;

        public AdminBookService(PersonalLibraryContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<BookResponse>> GetBooksAsync(int page = 1, int pageSize = 20, string? searchTerm = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.ToLower();
                query = query.Where(b => 
                    b.Title.ToLower().Contains(search) || 
                    b.Author.ToLower().Contains(search) ||
                    b.Genre.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            var books = await query
                .OrderByDescending(b => b.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookResponse
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    CoverImageUrl = b.CoverImageUrl,
                    Genre = b.Genre,
                    PageCount = b.PageCount,
                    PublishedYear = b.PublishedYear,
                    Rating = b.Rating,
                    Language = b.Language,
                    FileUrl = b.FileUrl,
                    TotalChapters = _context.BookContents.Count(c => c.BookId == b.BookId),
                    CreatedAt = b.CreatedAt,
                })
                .ToListAsync();

            return new PagedResult<BookResponse>
            {
                Items = books,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize,
            };
        }
   
        public async Task<CreateBookResponse> CreateBookAsync(CreateBookRequest request)
        {
          
            var book = new Book
            {
                Title = request.Title.Trim(),
                Author = request.Author.Trim(),
                Description = request.Description?.Trim(),
                CoverImageUrl = request.CoverImageUrl?.Trim(),
                Genre = request.Genre?.Trim(),
                PageCount = request.PageCount,
                PublishedYear = request.PublishedYear,
                Rating = request.Rating,
                Language = request.Language?.Trim() ?? "Tiếng Việt",
                FileUrl = request.FileUrl?.Trim(),
                CreatedAt = DateTime.Now,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync(); 

            int chaptersAdded = 0;

           
            if (request.Chapters != null && request.Chapters.Any())
            {
             
                var chapterNumbers = request.Chapters.Select(c => c.ChapterNumber).ToList();
                if (chapterNumbers.Distinct().Count() != chapterNumbers.Count)
                    throw new InvalidOperationException("Số chương bị trùng lặp.");

                var chapters = request.Chapters
                    .OrderBy(c => c.ChapterNumber)
                    .Select(c => new BookContent
                    {
                        BookId = book.BookId,
                        ChapterNumber = c.ChapterNumber,
                        ChapterTitle = c.ChapterTitle?.Trim(),
                        Content = c.Content,
                        WordCount = CountWords(c.Content),
                        CreatedAt = DateTime.Now,
                    }).ToList();

                _context.BookContents.AddRange(chapters);
                await _context.SaveChangesAsync();
                chaptersAdded = chapters.Count;
            }

            return new CreateBookResponse
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                ChaptersAdded = chaptersAdded,
                CreatedAt = book.CreatedAt ?? DateTime.Now,
                Message = chaptersAdded > 0
                    ? $"Tạo sách thành công với {chaptersAdded} chương."
                    : "Tạo sách thành công. Chưa có nội dung chương.",
            };
        }

        
        public async Task<BookResponse?> GetBookByIdAsync(int bookId)
        {
            var book = await _context.Books
                .Where(b => b.BookId == bookId)
                .Select(b => new BookResponse
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    CoverImageUrl = b.CoverImageUrl,
                    Genre = b.Genre,
                    PageCount = b.PageCount,
                    PublishedYear = b.PublishedYear,
                    Rating = b.Rating,
                    Language = b.Language,
                    FileUrl = b.FileUrl,
                    TotalChapters = _context.BookContents.Count(c => c.BookId == b.BookId),
                    CreatedAt = b.CreatedAt,
                })
                .FirstOrDefaultAsync();

            return book;
        }

     
        public async Task<BookResponse?> UpdateBookAsync(int bookId, UpdateBookRequest request)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return null;

          
            if (request.Title != null) book.Title = request.Title.Trim();
            if (request.Author != null) book.Author = request.Author.Trim();
            if (request.Description != null) book.Description = request.Description.Trim();
            if (request.CoverImageUrl != null) book.CoverImageUrl = request.CoverImageUrl.Trim();
            if (request.Genre != null) book.Genre = request.Genre.Trim();
            if (request.PageCount != null) book.PageCount = request.PageCount;
            if (request.PublishedYear != null) book.PublishedYear = request.PublishedYear;
            if (request.Rating != null) book.Rating = request.Rating;
            if (request.Language != null) book.Language = request.Language.Trim();
            if (request.FileUrl != null) book.FileUrl = request.FileUrl.Trim();

            await _context.SaveChangesAsync();

            return await GetBookByIdAsync(bookId);
        }

     
        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return false;

          
            var chapters = await _context.BookContents
                .Where(c => c.BookId == bookId)
                .ToListAsync();
            _context.BookContents.RemoveRange(chapters);

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

      
        public async Task<bool> BookExistsAsync(string title, string author)
        {
            return await _context.Books
                .AnyAsync(b =>
                    b.Title.ToLower() == title.Trim().ToLower() &&
                    b.Author.ToLower() == author.Trim().ToLower());
        }

       
        private static int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return text.Split(new[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
