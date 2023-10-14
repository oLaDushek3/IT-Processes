using System;
using System.Collections.Generic;

namespace ITProcesses.Models;

public partial class Archive
{
    public int Id { get; set; }

    public DateTime DateArchivedTimestamp { get; set; }

    public Guid? TaskId { get; set; }

    public virtual Task? Task { get; set; }
}
