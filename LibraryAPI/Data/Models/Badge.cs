namespace LibraryAPI.Data.Models;

public partial class Badge
{
    public int BadgeId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Icon { get; set; } = "🏅";

    // Loại điều kiện: "books_read" | "streak" | "pages_read" | "hours_read" | "night_read" | "genre_master"
    public string ConditionType { get; set; } = null!;

    // Ngưỡng cần đạt (vd: 1, 10, 100)
    public int Threshold { get; set; }

    // Thứ tự hiển thị
    public int DisplayOrder { get; set; }

    public virtual ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
}