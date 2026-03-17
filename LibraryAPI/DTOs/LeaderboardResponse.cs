namespace LibraryAPI.DTOs
{
    public class LeaderboardResponse
    {
        public string Type { get; set; } = null!; // "books" | "streak" | "pages" | "hours"
        public List<LeaderboardEntry> Entries { get; set; } = new();
        public int? CurrentUserRank { get; set; } // Rank của user đang đăng nhập
    }

}
