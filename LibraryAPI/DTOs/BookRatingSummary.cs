namespace LibraryAPI.DTOs
{
    public class BookRatingSummary
    {
        public int BookId { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public int? MyRating { get; set; }       // null = chưa rating
        public string? MyReview { get; set; }
        public bool CanRate { get; set; }         // true khi đọc ≥70%
        public List<BookRatingResponse> RecentReviews { get; set; } = new();
    }
}
