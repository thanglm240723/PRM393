using System.Text.Json.Serialization;

namespace LibraryAPI.DTOs
{
    public class UserResponse
    {
    
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("avatarUrl")]
        public string? AvatarUrl { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; } = "user";

        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;
    }
}