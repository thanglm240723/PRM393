using System;
using System.Collections.Generic;

namespace LibraryAPI.Data.Models;

public partial class Bookmark
{
    public int BookmarkId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public int? ContentId { get; set; }

    public int ChapterNumber { get; set; }

    public int Position { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual BookContent? Content { get; set; }

    public virtual User? User { get; set; }
}
