using LibraryAPI.Data;
using LibraryAPI.Data.Models;
using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Service
{
    public class ReadingProgressService : IReadingProgressService
    {
        private readonly PersonalLibraryContext _context;
        private readonly IGamificationService _gamification;

        public ReadingProgressService(
            PersonalLibraryContext context,
            IGamificationService gamification)
        {
            _context = context;
            _gamification = gamification;
        }

        public async Task<ReadingProgressResponse?> GetProgressAsync(int userId, int bookId)
        {
            var progress = await _context.ReadingProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.BookId == bookId);

            return progress == null ? null : MapToResponse(progress);
        }

        public async Task<ReadingProgressResponse> SaveProgressAsync(
            int userId, SaveProgressRequest request)
        {
           
            var totalChapters = await _context.BookContents
                .CountAsync(c => c.BookId == request.BookId);

            
            var percentage = totalChapters > 0
                ? Math.Round((decimal)request.CurrentChapter / totalChapters * 100, 2)
                : 0;

            
            var progress = await _context.ReadingProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.BookId == request.BookId);

            if (progress == null)
            {
                progress = new ReadingProgress
                {
                    UserId = userId,
                    BookId = request.BookId,
                    CurrentChapter = request.CurrentChapter,
                    CurrentPosition = request.CurrentPosition,
                    ProgressPercentage = percentage,
                    LastReadAt = DateTime.Now,
                };
                _context.ReadingProgresses.Add(progress);
            }
            else
            {
                progress.CurrentChapter = request.CurrentChapter;
                progress.CurrentPosition = request.CurrentPosition;
                progress.ProgressPercentage = percentage;
                progress.LastReadAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            
            var book = await _context.Books.FindAsync(request.BookId);
            await _gamification.ProcessReadingProgressAsync(
                userId: userId,
                bookId: request.BookId,
                progressPercentage: percentage,
                bookGenre: book?.Genre,
                bookPageCount: book?.PageCount);

            return MapToResponse(progress);
        }

        public async Task<List<ReadingProgressResponse>> GetAllProgressAsync(int userId)
        {
            var list = await _context.ReadingProgresses
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.LastReadAt)
                .ToListAsync();

            return list.Select(MapToResponse).ToList();
        }

        private static ReadingProgressResponse MapToResponse(ReadingProgress p) => new()
        {
            ProgressId = p.ProgressId,
            UserId = p.UserId ?? 0,
            BookId = p.BookId ?? 0,
            CurrentChapter = p.CurrentChapter,
            CurrentPosition = p.CurrentPosition,
            ProgressPercentage = p.ProgressPercentage,
            LastReadAt = p.LastReadAt,
        };
    }
}