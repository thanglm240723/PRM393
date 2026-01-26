namespace LibraryAPI.DTOs
{
    public class UserResponse
    {
        public int id { get; set; }
        public string username { get; set; }
        public string FullName { get; set; }
        public string email { get; set; }
        public string AvatarUrl { get; set; }
        public string Token { get; set; }

    }
}
