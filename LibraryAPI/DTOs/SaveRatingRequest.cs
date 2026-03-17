using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class SaveRatingRequest
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Số sao phải từ 1 đến 5")]
        public int Stars { get; set; }

        [MaxLength(2000)]
        public string? Review { get; set; }
    }
}
