using System;
using System.Collections.Generic;

namespace ITProcesses.Models;

public partial class TaskTag
{
    public int Id { get; set; }

    public Guid? TaskId { get; set; }

    public int? TagId { get; set; }

    public virtual Tag? Tag { get; set; }

    public virtual Tasks? Task { get; set; }
}
