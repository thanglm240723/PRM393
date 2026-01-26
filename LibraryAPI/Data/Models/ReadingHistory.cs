using System;
using System.Collections.Generic;

namespace LibraryAPI.Data.Models;

public partial class ReadingHistory
{
    public int HistoryId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public DateTime? ReadAt { get; set; }

    public int? MinutesRead { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
