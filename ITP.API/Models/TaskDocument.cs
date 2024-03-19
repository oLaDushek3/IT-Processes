using System;
using System.Collections.Generic;

namespace ITP.API.Models;

public partial class TaskDocument
{
    public int Id { get; set; }

    public Guid? TaskId { get; set; }

    public Guid? DocumentsId { get; set; }

    public virtual Document? Documents { get; set; }

    public virtual Task? Task { get; set; }
}
