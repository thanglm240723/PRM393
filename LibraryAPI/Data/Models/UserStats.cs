namespace LibraryAPI.Data.Models;

public partial class UserStats
{
    public int UserStatsId { get; set; }
    public int UserId { get; set; }

    // ── Thống kê đọc sách ──────────────────────────────────────────
    public int TotalBooksRead { get; set; } = 0;        // Số sách đã đọc ≥ 70%
    public int TotalBooksStarted { get; set; } = 0;     // Số sách đã mở đọc
    public int TotalPagesRead { get; set; } = 0;        // Tổng trang đã đọc
    public int TotalMinutesRead { get; set; } = 0;      // Tổng phút đọc
    public int TotalWordsRead { get; set; } = 0;        // Tổng từ đã đọc

    // ── Streak ─────────────────────────────────────────────────────
    public int CurrentStreak { get; set; } = 0;         // Chuỗi ngày hiện tại
    public int LongestStreak { get; set; } = 0;         // Chuỗi ngày dài nhất
    public DateTime? LastReadDate { get; set; }          // Ngày đọc gần nhất

    // ── Thể loại yêu thích ─────────────────────────────────────────
    public string? FavoriteGenre { get; set; }

    // ── Cấp bậc ────────────────────────────────────────────────────
    // "Mầm Đọc" | "Độc Giả Mới" | "Mọt Sách" | "Học Giả" | "Bậc Thầy"
    public string Rank { get; set; } = "Mầm Đọc";

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public virtual User? User { get; set; }
}