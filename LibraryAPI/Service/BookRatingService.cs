namespace LibraryAPI.Service
{
    using LibraryAPI.Data;
    using LibraryAPI.Data.Models;
    using LibraryAPI.DTOs;
    using LibraryAPI.Service.Interface;
    using Microsoft.EntityFrameworkCore;

    public class BookRatingService : IBookRatingService
    {
        private readonly PersonalLibraryContext _context;

        public BookRatingService(PersonalLibraryContext context)
        {
            _context = context;
        }

        public async Task<BookRatingResponse> SaveRatingAsync(int userId, SaveRatingRequest request)
        {
            var existing = await _context.BookRatings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == request.BookId);

            // Kiểm tra user đã đọc ≥70% chưa
            var isVerified = await _context.UserLibraries
                .AnyAsync(ul => ul.UserId == userId
                             && ul.BookId == request.BookId
                             && ul.IsCountedAsRead);

            if (existing == null)
            {
                existing = new BookRating
                {
                    UserId = userId,
                    BookId = request.BookId,
                    Stars = request.Stars,
                    Review = request.Review?.Trim(),
                    IsVerifiedReader = isVerified,
                    CreatedAt = DateTime.Now,
                };
                _context.BookRatings.Add(existing);
            }
            else
            {
                // Update
                existing.Stars = request.Stars;
                existing.Review = request.Review?.Trim();
                existing.IsVerifiedReader = isVerified;
                existing.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            // Cập nhật rating trung bình của sách
            await UpdateBookAverageRatingAsync(request.BookId);

            var user = await _context.Users.FindAsync(userId);
            return new BookRatingResponse
            {
                RatingId = existing.RatingId,
                BookId = existing.BookId,
                UserId = existing.UserId,
                Username = user?.Username,
                Stars = existing.Stars,
                Review = existing.Review,
                IsVerifiedReader = existing.IsVerifiedReader,
                CreatedAt = existing.CreatedAt,
            };
        }

        public async Task<BookRatingSummary> GetBookRatingSummaryAsync(int userId, int bookId)
        {
            var ratings = await _context.BookRatings
                .Include(r => r.User)
                .Where(r => r.BookId == bookId)
                .ToListAsync();

            var myRating = ratings.FirstOrDefault(r => r.UserId == userId);

            // Có thể rate nếu đã đọc ≥70%
            var canRate = await _context.UserLibraries
                .AnyAsync(ul => ul.UserId == userId
                             && ul.BookId == bookId
                             && ul.IsCountedAsRead);

            var avg = ratings.Any()
                ? Math.Round(ratings.Average(r => r.Stars), 1)
                : 0.0;

            // Lấy 5 review gần nhất (ưu tiên verified reader)
            var recent = ratings
                .OrderByDescending(r => r.IsVerifiedReader)
                .ThenByDescending(r => r.CreatedAt)
                .Take(5)
                .Select(r => new BookRatingResponse
                {
                    RatingId = r.RatingId,
                    BookId = r.BookId,
                    UserId = r.UserId,
                    Username = r.User?.Username,
                    Stars = r.Stars,
                    Review = r.Review,
                    IsVerifiedReader = r.IsVerifiedReader,
                    CreatedAt = r.CreatedAt,
                }).ToList();

            return new BookRatingSummary
            {
                BookId = bookId,
                AverageRating = avg,
                TotalRatings = ratings.Count,
                MyRating = myRating?.Stars,
                MyReview = myRating?.Review,
                CanRate = canRate,
                RecentReviews = recent,
            };
        }

        public async Task<bool> DeleteRatingAsync(int userId, int bookId)
        {
            var rating = await _context.BookRatings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);

            if (rating == null) return false;

            _context.BookRatings.Remove(rating);
            await _context.SaveChangesAsync();
            await UpdateBookAverageRatingAsync(bookId);
            return true;
        }

        // Cập nhật Books.Rating = trung bình tất cả ratings
        private async Task UpdateBookAverageRatingAsync(int bookId)
        {
            var avg = await _context.BookRatings
                .Where(r => r.BookId == bookId)
                .AverageAsync(r => (double?)r.Stars);

            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return;

            book.Rating = avg.HasValue
                ? (decimal)Math.Round(avg.Value, 2)
                : 0;

            await _context.SaveChangesAsync();
        }
    }
}