namespace LibraryAPI.DTOs
{
    public class LeaderboardEntry
    {
        public int Rank { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string RankTitle { get; set; } = null!; // Cấp bậc
        public long Value { get; set; }               // Giá trị (số sách / streak / trang)
        public string ValueLabel { get; set; } = null!; // "12 cuốn" / "7 ngày" / "1.2K trang"
        public bool IsCurrentUser { get; set; }
    }
}
