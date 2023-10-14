using System;
using System.Collections.Generic;

namespace ITProcesses.Models;

public partial class TaskTag
{
    public int Id { get; set; }

    public Guid? Task { get; set; }

    public int? Tag { get; set; }

    public virtual Tag? TagNavigation { get; set; }

    public virtual Task? TaskNavigation { get; set; }
}
