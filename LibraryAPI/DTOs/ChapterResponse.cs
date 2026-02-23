namespace LibraryAPI.DTOs
{
    public class ChapterResponse
    {
        public int ContentId { get; set; }
        public int BookId { get; set; }
        public int ChapterNumber { get; set; }
        public string? ChapterTitle { get; set; }
        public string Content { get; set; } = null!;
        public int? WordCount { get; set; }
    }
}
