namespace LibraryAPI.DTOs
{
    public class BookResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Genre { get; set; }
        public int? PageCount { get; set; }
        public int? PublishedYear { get; set; }
        public decimal? Rating { get; set; }
        public string? Language { get; set; }
        public string? FileUrl { get; set; }
        public int TotalChapters { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
