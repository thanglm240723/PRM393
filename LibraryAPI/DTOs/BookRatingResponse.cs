namespace LibraryAPI.DTOs
{
    public class BookRatingResponse
    {
        public int RatingId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public int Stars { get; set; }
        public string? Review { get; set; }
        public bool IsVerifiedReader { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
