using LibraryAPI.Data;
using LibraryAPI.Data.Models;
using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Service
{
    public class UserLibraryService : IUserLibraryService
    {
        private readonly PersonalLibraryContext _context;

        public UserLibraryService(PersonalLibraryContext context)
        {
            _context = context;
        }

        public async Task<bool> ToggleFavoriteAsync(int userId, int bookId)
        {
            var record = await _context.UserLibraries
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);

            if (record == null)
            {
                record = new UserLibrary
                {
                    UserId = userId,
                    BookId = bookId,
                    IsFavorite = true,
                    AddedAt = DateTime.UtcNow
                };
                _context.UserLibraries.Add(record);
            }
            else
            {
                // Đảo ngược trạng thái hiện tại (nếu null thì mặc định false rồi đảo thành true)
                record.IsFavorite = !(record.IsFavorite ?? false);
            }

            await _context.SaveChangesAsync();
            return record.IsFavorite ?? false;
        }

        public async Task<List<BookResponse>> GetSavedBooksAsync(int userId)
        {
            var savedBooks = await _context.UserLibraries
                .Where(x => x.UserId == userId && x.IsFavorite == true)
                .Include(x => x.Book)
                .Select(x => x.Book)
                .ToListAsync();

            if (savedBooks == null || !savedBooks.Any()) return new List<BookResponse>();

            return savedBooks.Select(b => new BookResponse
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                CoverImageUrl = b.CoverImageUrl,
                Rating = b.Rating,
                Genre = b.Genre
            }).ToList();
        }

        public async Task<bool> CheckIsSavedAsync(int userId, int bookId)
        {
            var record = await _context.UserLibraries
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);
            return record?.IsFavorite ?? false;
        }
    }
}