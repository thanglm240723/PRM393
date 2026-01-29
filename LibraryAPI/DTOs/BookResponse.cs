namespace LibraryAPI.DTOs
{
    public class BookResponse
    {

        public int BookId { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string? Description { get; set; }

        public string? CoverImageUrl { get; set; }

        public string? Genre { get; set; }

        public int? PageCount { get; set; }      

        public decimal? Rating { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
