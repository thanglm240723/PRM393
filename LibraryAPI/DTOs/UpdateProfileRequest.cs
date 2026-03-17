using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class UpdateProfileRequest
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }
    }
}
