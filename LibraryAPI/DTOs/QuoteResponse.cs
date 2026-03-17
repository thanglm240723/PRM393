namespace LibraryAPI.DTOs
{
    public class QuoteResponse
    {
        public int QuoteId { get; set; }
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookCover { get; set; }
        public int? ContentId { get; set; }
        public string? ChapterTitle { get; set; }
        public string QuoteText { get; set; } = null!;
        public string? PersonalNote { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
