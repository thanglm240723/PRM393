namespace LibraryAPI.Data.Models;

public partial class UserLibrary
{
    public int UserLibraryId { get; set; }
    public int? UserId { get; set; }
    public int? BookId { get; set; }
    public DateTime? AddedAt { get; set; }
    public bool? IsFavorite { get; set; }
    public string? Status { get; set; }  
    public DateTime? CompletedAt { get; set; }

    public bool IsCountedAsRead { get; set; } = false;

    public virtual Book? Book { get; set; }
    public virtual User? User { get; set; }
}