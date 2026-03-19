namespace LibraryAPI.DTOs;


public class AdminUserStatsSummary
{
    public int TotalBooksRead { get; set; }
    public int TotalBooksStarted { get; set; }
    public int TotalPagesRead { get; set; }
    public int TotalMinutesRead { get; set; }
    public int TotalWordsRead { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateTime? LastReadDate { get; set; }
    public string? FavoriteGenre { get; set; }
    public string Rank { get; set; } = "Mầm Đọc";
    public DateTime? StatsUpdatedAt { get; set; }
}
