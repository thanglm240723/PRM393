public class ReadingHistoryDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public DateTime ReadAt { get; set; }
    public int Progress { get; set; } // Nếu có lưu phần trăm/trang đã đọc
}