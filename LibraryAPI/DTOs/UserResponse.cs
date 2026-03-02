namespace LibraryAPI.DTOs
{
    public class UserResponse
    {
        public int id { get; set; }
        public string username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = "user"; 
        public string Token { get; set; } = null!;
    }
}