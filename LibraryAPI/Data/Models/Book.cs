using System;
using System.Collections.Generic;

namespace LibraryAPI.Data.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Description { get; set; }

    public string? CoverImageUrl { get; set; }

    public string? Genre { get; set; }

    public int? PageCount { get; set; }

    public int? PublishedYear { get; set; }

    public decimal? Rating { get; set; }

    public string? Language { get; set; }

    public string? FileUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BookContent> BookContents { get; set; } = new List<BookContent>();

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual ICollection<Highlight> Highlights { get; set; } = new List<Highlight>();

    public virtual ICollection<ReadingHistory> ReadingHistories { get; set; } = new List<ReadingHistory>();

    public virtual ICollection<ReadingProgress> ReadingProgresses { get; set; } = new List<ReadingProgress>();

    public virtual ICollection<UserLibrary> UserLibraries { get; set; } = new List<UserLibrary>();
    public virtual ICollection<BookRating> BookRatings { get; set; } = new List<BookRating>();
}
