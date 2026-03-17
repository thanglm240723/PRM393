namespace LibraryAPI.DTOs
{
    // ── Trả về sau mỗi lần SaveProgress ──────────────────────────────
    // Flutter dùng để hiện popup "Bạn vừa nhận badge!" hoặc "Lên rank!"
    public class GamificationResult
    {
        // Badges mới nhận được trong lần đọc này
        public List<BadgeDto> NewBadges { get; set; } = new();

        // Nếu vừa lên rank mới
        public string? NewRank { get; set; }

        // Streak hiện tại
        public int CurrentStreak { get; set; }

        // Sách này vừa được đánh dấu hoàn thành (≥70%) không
        public bool BookJustCompleted { get; set; }

        // Tổng sách đã đọc xong
        public int TotalBooksRead { get; set; }

        // Có gì đặc biệt không (để Flutter quyết định có show popup không)
        public bool HasAnyReward => NewBadges.Any() || NewRank != null || BookJustCompleted;
    }
}
