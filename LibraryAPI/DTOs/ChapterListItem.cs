namespace LibraryAPI.DTOs
{
    public class ChapterListItem
    {
        public int ContentId { get; set; }
        public int ChapterNumber { get; set; }
        public string? ChapterTitle { get; set; }
        public int? WordCount { get; set; }
    }
}
