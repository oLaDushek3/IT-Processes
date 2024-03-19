using System;
using System.Collections.Generic;

namespace ITP.API.Models;

public partial class UsersTask
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? TaskId { get; set; }

    public string? UserComments { get; set; }

    public virtual Task? Task { get; set; }

    public virtual User? User { get; set; }
}
