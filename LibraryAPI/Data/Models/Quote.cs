namespace LibraryAPI.Data.Models;

public partial class Quote
{
    public int QuoteId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int? ContentId { get; set; }   // Chương chứa quote này

    // Nội dung quote (câu/đoạn user chọn)
    public string QuoteText { get; set; } = null!;

    // Ghi chú cá nhân của user
    public string? PersonalNote { get; set; }

    // Vị trí trong chương (để highlight lại khi mở)
    public int? StartPosition { get; set; }
    public int? EndPosition { get; set; }

    // Public = hiện ở leaderboard/cộng đồng, Private = chỉ user thấy
    public bool IsPublic { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual User? User { get; set; }
    public virtual Book? Book { get; set; }
    public virtual BookContent? Content { get; set; }
}