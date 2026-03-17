using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải ít nhất 6 ký tự")]
        public string NewPassword { get; set; } = null!;
    }

}
