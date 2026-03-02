// ── DTOs/ReadingProgressDto.cs ────────────────────────────────────────
namespace LibraryAPI.DTOs
{
    public class ReadingProgressResponse
    {
        public int ProgressId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int CurrentChapter { get; set; }
        public int? CurrentPosition { get; set; }
        public decimal? ProgressPercentage { get; set; }
        public DateTime? LastReadAt { get; set; }
    }

    public class SaveProgressRequest
    {
        public int BookId { get; set; }
        public int CurrentChapter { get; set; }
        public int CurrentPosition { get; set; } = 0;
    }
}