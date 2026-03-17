namespace LibraryAPI.DTOs
{
    public class BadgeDto
    {
        public int BadgeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Icon { get; set; } = "🏅";
        public string ConditionType { get; set; } = null!;
        public int Threshold { get; set; }
        public DateTime? EarnedAt { get; set; }   // null = chưa nhận
        public bool IsEarned => EarnedAt != null;
    }
}
