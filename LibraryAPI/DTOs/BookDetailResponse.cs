namespace LibraryAPI.DTOs
{
    public class BookDetailResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Genre { get; set; }
        public int? PageCount { get; set; }
        public int? PublishedYear { get; set; }
        public decimal? Rating { get; set; }
        public string? Language { get; set; }
        public string? FileUrl { get; set; }       // Dùng cho nút Tải về
        public DateTime? CreatedAt { get; set; }
        public int TotalChapters { get; set; }     // Đếm từ BookContents
    }
}