using System;
using System.Collections.Generic;

namespace ITP.API.Models;

public partial class TaskTag
{
    public int Id { get; set; }

    public Guid? TaskId { get; set; }

    public int? TagId { get; set; }

    public virtual Tag? Tag { get; set; }

    public virtual Task? Task { get; set; }
}
