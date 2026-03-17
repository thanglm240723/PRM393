namespace LibraryAPI.Data.Models;

public partial class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public string Role { get; set; } = "user";
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public virtual ICollection<Highlight> Highlights { get; set; } = new List<Highlight>();
    public virtual ICollection<ReadingHistory> ReadingHistories { get; set; } = new List<ReadingHistory>();
    public virtual ICollection<ReadingProgress> ReadingProgresses { get; set; } = new List<ReadingProgress>();
    public virtual ICollection<UserLibrary> UserLibraries { get; set; } = new List<UserLibrary>();

    public virtual UserStats? Stats { get; set; }
    public virtual ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
    public virtual ICollection<BookRating> BookRatings { get; set; } = new List<BookRating>();
    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}