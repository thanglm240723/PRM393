public class BookmarkDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public int? ChapterId { get; set; }
    public int? PageNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}