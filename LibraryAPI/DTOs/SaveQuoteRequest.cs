using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class SaveQuoteRequest
    {
        [Required]
        public int BookId { get; set; }

        public int? ContentId { get; set; }      // Chapter chứa quote

        [Required(ErrorMessage = "Nội dung quote không được để trống")]
        public string QuoteText { get; set; } = null!;

        [MaxLength(1000)]
        public string? PersonalNote { get; set; }

        public int? StartPosition { get; set; }
        public int? EndPosition { get; set; }

        public bool IsPublic { get; set; } = false;
    }
}
