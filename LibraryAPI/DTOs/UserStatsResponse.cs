namespace LibraryAPI.DTOs
{
    public class UserStatsResponse
    {
        public int UserId { get; set; }

        // Thống kê đọc sách
        public int TotalBooksRead { get; set; }
        public int TotalBooksStarted { get; set; }
        public int TotalPagesRead { get; set; }
        public int TotalMinutesRead { get; set; }
        public int TotalWordsRead { get; set; }

        // Streak
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateTime? LastReadDate { get; set; }

        // Cấp bậc
        public string Rank { get; set; } = "Mầm Đọc";
        public string? FavoriteGenre { get; set; }

        // Tiến trình lên rank tiếp theo
        public int BooksToNextRank { get; set; }
        public string? NextRank { get; set; }

        // Giờ đọc (tính từ TotalMinutesRead)
        public double TotalHoursRead => Math.Round(TotalMinutesRead / 60.0, 1);

        // Số từ đã đọc (quy đổi để hiển thị thú vị)
        // Ví dụ: "Tương đương 3 cuốn Harry Potter"
        public string TotalWordsReadLabel =>
            TotalWordsRead >= 1_000_000
                ? $"{TotalWordsRead / 1_000_000.0:F1}M từ"
                : TotalWordsRead >= 1_000
                    ? $"{TotalWordsRead / 1_000}K từ"
                    : $"{TotalWordsRead} từ";

        // Danh sách badges
        public List<BadgeDto> Badges { get; set; } = new();
    }
}
