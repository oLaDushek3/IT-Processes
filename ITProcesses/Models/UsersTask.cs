using System;
using System.Collections.Generic;

namespace ITProcesses.Models;

public partial class UsersTask
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? TaskId { get; set; }

    public string? UserComments { get; set; }

    public int? TimeCount { get; set; }

    public virtual Tasks? Task { get; set; }

    public virtual User? User { get; set; }
}
