using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs;


public class AdminUpdateUserRequest
{
    [MaxLength(100)]
    public string? FullName { get; set; }

    [MaxLength(100)]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? AvatarUrl { get; set; }

    [MaxLength(20)]
    [RegularExpression(@"^(user|admin)$", ErrorMessage = "Role phải là 'user' hoặc 'admin'")]
    public string? Role { get; set; }
}
