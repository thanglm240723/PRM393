using System;
using System.Collections.Generic;

namespace LibraryAPI.Data.Models;

public partial class BookContent
{
    public int ContentId { get; set; }

    public int? BookId { get; set; }

    public int ChapterNumber { get; set; }

    public string? ChapterTitle { get; set; }

    public string Content { get; set; } = null!;

    public int? WordCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual ICollection<Highlight> Highlights { get; set; } = new List<Highlight>();

    public virtual ICollection<ReadingProgress> ReadingProgresses { get; set; } = new List<ReadingProgress>();
}
