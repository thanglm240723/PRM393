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

        /*public async Task<ReadingProgressResponse> SaveProgressAsync(
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
        }*/

        public async Task<ReadingProgressResponse> SaveProgressAsync(
            int userId, SaveProgressRequest request)
        {
            var totalChapters = await _context.BookContents
                .CountAsync(c => c.BookId == request.BookId);

            var percentage = totalChapters > 0
                ? Math.Round((decimal)request.CurrentChapter / totalChapters * 100, 2)
                : 0;

            // 1. CẬP NHẬT HOẶC THÊM MỚI TIẾN TRÌNH (ReadingProgresses)
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

            // 2. THÊM VÀO LỊCH SỬ ĐỌC (ReadingHistories)
            // Mỗi lần lưu tiến trình sẽ tự động tạo 1 mốc lịch sử mới
            var history = new ReadingHistory
            {
                UserId = userId,
                BookId = request.BookId,
                ReadAt = DateTime.Now
                // Bỏ qua trường MinutesRead ở đây vì SaveProgressRequest hiện chưa có trường này
            };
            _context.ReadingHistories.Add(history);

            // 3. TỰ ĐỘNG LƯU DẤU TRANG KHI THOÁT (Bookmarks)
            // Tìm xem đã có Bookmark tự động nào cho sách này chưa
            var autoBookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == request.BookId && b.Note == "Auto-Save");

            if (autoBookmark == null)
            {
                autoBookmark = new Bookmark
                {
                    UserId = userId,
                    BookId = request.BookId,
                    ChapterNumber = request.CurrentChapter,
                    Position = request.CurrentPosition,
                    Note = "Auto-Save",
                    CreatedAt = DateTime.Now
                };
                _context.Bookmarks.Add(autoBookmark);
            }
            else
            {
                // Nếu có rồi thì cập nhật lại vị trí trang/chương mới nhất
                autoBookmark.ChapterNumber = request.CurrentChapter;
                autoBookmark.Position = request.CurrentPosition;
                autoBookmark.CreatedAt = DateTime.Now;
            }

            // Lưu toàn bộ thay đổi (Progress, History, Bookmark) vào Database cùng 1 lúc
            await _context.SaveChangesAsync();

            // 4. XỬ LÝ ĐIỂM THƯỞNG GAMIFICATION
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

        public async Task<List<ReadingHistoryDto>> GetReadingHistoryAsync(int userId)
        {
            return await _context.ReadingHistories
                .Include(h => h.Book)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.ReadAt)
                .Select(h => new ReadingHistoryDto
                {
                    BookId = h.BookId ?? 0, // Xử lý int? sang int
                    Title = h.Book != null ? h.Book.Title : "Chưa rõ", // Kiểm tra null cho Book
                    CoverImage = h.Book != null ? h.Book.CoverImageUrl : null, // Map đúng tên CoverImageUrl
                    ReadAt = h.ReadAt ?? DateTime.Now, // Xử lý DateTime?
                                                       // Progress = h.MinutesRead ?? 0 // Có thể dùng MinutesRead nếu muốn hiển thị thời gian đã đọc
                })
                .ToListAsync();
        }

        public async Task<List<BookmarkDto>> GetBookmarksAsync(int userId)
        {
            return await _context.Bookmarks
                .Include(b => b.Book)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookmarkDto
                {
                    Id = b.BookmarkId, // Map đúng BookmarkId
                    BookId = b.BookId ?? 0, // Xử lý int?
                    Title = b.Book != null ? b.Book.Title : "Chưa rõ",
                    CoverImage = b.Book != null ? b.Book.CoverImageUrl : null, // Map đúng CoverImageUrl
                    ChapterId = b.ChapterNumber, // Dùng ChapterNumber thay thế
                    PageNumber = b.Position,     // Dùng Position để lưu vị trí/trang
                    CreatedAt = b.CreatedAt ?? DateTime.Now // Xử lý DateTime?
                })
                .ToListAsync();
        }
    }
}