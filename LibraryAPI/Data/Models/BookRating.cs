namespace LibraryAPI.Data.Models;

public partial class BookRating
{
    public int RatingId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }

    // 1 - 5 sao
    public int Stars { get; set; }

    // Review text (optional)
    public string? Review { get; set; }

    // Chỉ người đọc ≥ 70% mới được rate
    public bool IsVerifiedReader { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public virtual User? User { get; set; }
    public virtual Book? Book { get; set; }
}
