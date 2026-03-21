
// Service/GamificationService.cs
// FIX: TotalPagesRead tính theo % đọc thực tế, không cộng toàn bộ pageCount

using LibraryAPI.Data;
using LibraryAPI.Data.Models;
using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Service
{
    public class GamificationService : IGamificationService
    {
        private readonly PersonalLibraryContext _context;

        private static readonly List<(int MinBooks, string Rank)> RankTiers = new()
        {
            (0,   "Mầm Đọc"),
            (1,   "Độc Giả Mới"),
            (5,   "Mọt Sách"),
            (20,  "Học Giả"),
            (50,  "Bậc Thầy"),
            (100, "Huyền Thoại"),
        };

        private const decimal CompletionThreshold = 70m;

        public GamificationService(PersonalLibraryContext context)
        {
            _context = context;
        }

        public async Task<GamificationResult> ProcessReadingProgressAsync(
            int userId, int bookId, decimal progressPercentage,
            string? bookGenre, int? bookPageCount)
        {
            var result = new GamificationResult();
            var stats = await GetOrCreateStatsAsync(userId);

            // 1. Streak
            UpdateStreak(stats);
            result.CurrentStreak = stats.CurrentStreak;

            // 2. Đánh dấu bắt đầu
            await EnsureBookStartedAsync(userId, bookId, stats);

            // 3. Kiểm tra hoàn thành ≥70%
            bool justCompleted = await CheckAndMarkBookCompletedAsync(
                userId, bookId, progressPercentage, bookGenre, bookPageCount, stats);

            result.BookJustCompleted = justCompleted;
            result.TotalBooksRead = stats.TotalBooksRead;

            // 4. ── FIX: Cập nhật TotalPagesRead theo % thực tế ────────
            await UpdatePagesReadAsync(userId, bookId, progressPercentage, bookPageCount, stats);

            // 5. Cập nhật TotalWordsRead
            await UpdateWordsReadAsync(userId, bookId, progressPercentage, stats);

            // 6. Cập nhật FavoriteGenre
            if (justCompleted && !string.IsNullOrEmpty(bookGenre))
                await UpdateFavoriteGenreAsync(userId, stats);

            // 7. Rank
            var newRank = CalculateRank(stats.TotalBooksRead);
            if (newRank != stats.Rank)
            {
                result.NewRank = newRank;
                stats.Rank = newRank;
            }

            // 8. Badges
            result.NewBadges = await CheckAndAwardBadgesAsync(userId, stats);

            stats.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<UserStatsResponse> GetUserStatsAsync(int userId)
        {
            var stats = await GetOrCreateStatsAsync(userId);

            // TotalMinutesRead: lấy từ ReadingHistory (source of truth)
            var totalMinutes = await _context.ReadingHistories
                .Where(h => h.UserId == userId)
                .SumAsync(h => (int?)h.MinutesRead ?? 0);
            stats.TotalMinutesRead = totalMinutes;

            await _context.SaveChangesAsync();

            var badges = await GetUserBadgesAsync(userId);
            var nextTier = RankTiers.FirstOrDefault(r => r.MinBooks > stats.TotalBooksRead);

            return new UserStatsResponse
            {
                UserId = userId,
                TotalBooksRead = stats.TotalBooksRead,
                TotalBooksStarted = stats.TotalBooksStarted,
                TotalPagesRead = stats.TotalPagesRead,
                TotalMinutesRead = totalMinutes,
                TotalWordsRead = stats.TotalWordsRead,
                CurrentStreak = stats.CurrentStreak,
                LongestStreak = stats.LongestStreak,
                LastReadDate = stats.LastReadDate,
                Rank = stats.Rank,
                FavoriteGenre = stats.FavoriteGenre,
                NextRank = nextTier == default ? null : nextTier.Rank,
                BooksToNextRank = nextTier == default ? 0 : nextTier.MinBooks - stats.TotalBooksRead,
                Badges = badges,
            };
        }

        public async Task<List<BadgeDto>> GetUserBadgesAsync(int userId)
        {
            var allBadges = await _context.Badges.OrderBy(b => b.DisplayOrder).ToListAsync();
            var earnedBadges = await _context.UserBadges
                .Where(ub => ub.UserId == userId)
                .ToDictionaryAsync(ub => ub.BadgeId, ub => ub.EarnedAt);

            return allBadges.Select(b => new BadgeDto
            {
                BadgeId = b.BadgeId,
                Name = b.Name,
                Description = b.Description,
                Icon = b.Icon,
                ConditionType = b.ConditionType,
                Threshold = b.Threshold,
                EarnedAt = earnedBadges.TryGetValue(b.BadgeId, out var e) ? e : null,
            }).ToList();
        }

        public async Task<LeaderboardResponse> GetLeaderboardAsync(
            string type, int? currentUserId, int top = 20)
        {
            var query = _context.UserStats.Include(s => s.User).AsQueryable();

            IQueryable<UserStats> ordered = type switch
            {
                "streak" => query.OrderByDescending(s => s.LongestStreak),
                "pages" => query.OrderByDescending(s => s.TotalPagesRead),
                "hours" => query.OrderByDescending(s => s.TotalMinutesRead),
                _ => query.OrderByDescending(s => s.TotalBooksRead),
            };

            var topEntries = await ordered.Take(top).ToListAsync();

            int? currentUserRank = null;
            if (currentUserId.HasValue)
            {
                var all = await ordered.ToListAsync();
                var idx = all.FindIndex(s => s.UserId == currentUserId.Value);
                if (idx >= 0) currentUserRank = idx + 1;
            }

            var entries = topEntries.Select((s, i) => new LeaderboardEntry
            {
                Rank = i + 1,
                UserId = s.UserId,
                Username = s.User?.Username ?? "unknown",
                FullName = s.User?.FullName,
                AvatarUrl = s.User?.AvatarUrl,
                RankTitle = s.Rank,
                IsCurrentUser = s.UserId == currentUserId,
                Value = type switch
                {
                    "streak" => s.LongestStreak,
                    "pages" => s.TotalPagesRead,
                    "hours" => s.TotalMinutesRead / 60,
                    _ => s.TotalBooksRead,
                },
                ValueLabel = type switch
                {
                    "streak" => $"{s.LongestStreak} ngày",
                    "pages" => s.TotalPagesRead >= 1000
                        ? $"{s.TotalPagesRead / 1000.0:F1}K trang"
                        : $"{s.TotalPagesRead} trang",
                    "hours" => $"{s.TotalMinutesRead / 60} giờ",
                    _ => $"{s.TotalBooksRead} cuốn",
                },
            }).ToList();

            return new LeaderboardResponse
            {
                Type = type,
                Entries = entries,
                CurrentUserRank = currentUserRank,
            };
        }

        // ─────────────────────────────────────────────────────────────
        //  PRIVATE HELPERS
        // ─────────────────────────────────────────────────────────────

        private async Task<UserStats> GetOrCreateStatsAsync(int userId)
        {
            var stats = await _context.UserStats.FirstOrDefaultAsync(s => s.UserId == userId);
            if (stats == null)
            {
                stats = new UserStats { UserId = userId, Rank = "Mầm Đọc", UpdatedAt = DateTime.Now };
                _context.UserStats.Add(stats);
                await _context.SaveChangesAsync();
            }
            return stats;
        }

        private static void UpdateStreak(UserStats stats)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var lastRead = stats.LastReadDate.HasValue
                ? DateOnly.FromDateTime(stats.LastReadDate.Value)
                : (DateOnly?)null;

            if (lastRead == null)
            {
                stats.CurrentStreak = 1;
                stats.LongestStreak = 1;
            }
            else if (lastRead == today) { /* không thay đổi */ }
            else if (lastRead == today.AddDays(-1))
            {
                stats.CurrentStreak++;
                if (stats.CurrentStreak > stats.LongestStreak)
                    stats.LongestStreak = stats.CurrentStreak;
            }
            else { stats.CurrentStreak = 1; }

            stats.LastReadDate = DateTime.Today;
        }

        private async Task EnsureBookStartedAsync(int userId, int bookId, UserStats stats)
        {
            var lib = await _context.UserLibraries
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.BookId == bookId);

            if (lib == null)
            {
                _context.UserLibraries.Add(new UserLibrary
                {
                    UserId = userId,
                    BookId = bookId,
                    AddedAt = DateTime.Now,
                    Status = "Reading",
                    IsFavorite = false,
                    IsCountedAsRead = false,
                });
                stats.TotalBooksStarted++;
            }
            else if (lib.Status == "Want to Read")
            {
                lib.Status = "Reading";
                if (!lib.IsCountedAsRead) stats.TotalBooksStarted++;
            }
        }

        private async Task<bool> CheckAndMarkBookCompletedAsync(
            int userId, int bookId, decimal progressPercentage,
            string? bookGenre, int? bookPageCount, UserStats stats)
        {
            if (progressPercentage < CompletionThreshold) return false;

            var lib = await _context.UserLibraries
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.BookId == bookId);

            if (lib == null || lib.IsCountedAsRead) return false;

            lib.Status = "Completed";
            lib.CompletedAt = DateTime.Now;
            lib.IsCountedAsRead = true;
            stats.TotalBooksRead++;

            return true;
        }

        // ── FIX: Tính TotalPagesRead theo % thực tế ─────────────────
        // Chỉ cộng delta (phần mới đọc thêm), không cộng toàn bộ pageCount
        private async Task UpdatePagesReadAsync(
            int userId, int bookId, decimal progressPercentage,
            int? bookPageCount, UserStats stats)
        {
            if (bookPageCount == null || bookPageCount <= 0) return;

            // Tính số trang đã đọc theo % hiện tại
            var pagesNow = (int)(bookPageCount.Value * progressPercentage / 100);

            // Lấy progress cũ để tính delta
            var oldProgress = await _context.ReadingProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.BookId == bookId);
            var oldPercentage = oldProgress?.ProgressPercentage ?? 0;
            var pagesOld = (int)(bookPageCount.Value * oldPercentage / 100);

            var deltaPages = pagesNow - pagesOld;
            if (deltaPages > 0)
                stats.TotalPagesRead += deltaPages;
        }

        private async Task UpdateWordsReadAsync(
            int userId, int bookId, decimal progressPercentage, UserStats stats)
        {
            var totalWords = await _context.BookContents
                .Where(c => c.BookId == bookId)
                .SumAsync(c => (int?)c.WordCount ?? 0);

            if (totalWords <= 0) return;

            var wordsNow = (int)(totalWords * progressPercentage / 100);

            var oldProgress = await _context.ReadingProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.BookId == bookId);
            var oldPct = oldProgress?.ProgressPercentage ?? 0;
            var wordsOld = (int)(totalWords * oldPct / 100);

            var delta = wordsNow - wordsOld;
            if (delta > 0) stats.TotalWordsRead += delta;
        }

        private async Task UpdateFavoriteGenreAsync(int userId, UserStats stats)
        {
            var genreCount = await _context.UserLibraries
                .Where(ul => ul.UserId == userId && ul.IsCountedAsRead)
                .Include(ul => ul.Book)
                .GroupBy(ul => ul.Book!.Genre)
                .Select(g => new { Genre = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefaultAsync();

            if (genreCount?.Genre != null)
                stats.FavoriteGenre = genreCount.Genre;
        }

        private static string CalculateRank(int totalBooksRead)
        {
            return RankTiers
                .Where(r => totalBooksRead >= r.MinBooks)
                .OrderByDescending(r => r.MinBooks)
                .First().Rank;
        }

        private async Task<List<BadgeDto>> CheckAndAwardBadgesAsync(int userId, UserStats stats)
        {
            var newBadges = new List<BadgeDto>();
            var earnedIds = await _context.UserBadges
                .Where(ub => ub.UserId == userId).Select(ub => ub.BadgeId).ToListAsync();
            var pending = await _context.Badges
                .Where(b => !earnedIds.Contains(b.BadgeId)).ToListAsync();

            foreach (var badge in pending)
            {
                bool ok = badge.ConditionType switch
                {
                    "books_read" => stats.TotalBooksRead >= badge.Threshold,
                    "streak" => stats.LongestStreak >= badge.Threshold,
                    "pages_read" => stats.TotalPagesRead >= badge.Threshold,
                    "hours_read" => stats.TotalMinutesRead >= badge.Threshold * 60,
                    _ => false,
                };
                if (!ok) continue;

                _context.UserBadges.Add(new UserBadge
                {
                    UserId = userId,
                    BadgeId = badge.BadgeId,
                    EarnedAt = DateTime.Now
                });
                newBadges.Add(new BadgeDto
                {
                    BadgeId = badge.BadgeId,
                    Name = badge.Name,
                    Description = badge.Description,
                    Icon = badge.Icon,
                    ConditionType = badge.ConditionType,
                    Threshold = badge.Threshold,
                    EarnedAt = DateTime.Now,
                });
            }
            return newBadges;
        }
    }
}