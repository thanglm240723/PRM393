using System;
using System.Collections.Generic;

namespace LibraryAPI.Data.Models;

public partial class ReadingProgress
{
    public int ProgressId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public int? ContentId { get; set; }

    public int CurrentChapter { get; set; }

    public int? CurrentPosition { get; set; }

    public decimal? ProgressPercentage { get; set; }

    public DateTime? LastReadAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual BookContent? Content { get; set; }

    public virtual User? User { get; set; }
}
