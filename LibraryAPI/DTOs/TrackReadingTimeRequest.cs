using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class TrackReadingTimeRequest
    {
        [Required]
        public int BookId { get; set; }

        [Range(1, 1440, ErrorMessage = "Số phút phải từ 1 đến 1440")]
        public int MinutesRead { get; set; }
    }
}
