using System;
using System.Collections.Generic;

namespace ITP.API.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Archived { get; set; }

    public Guid? UserId { get; set; }

    public int? Test { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual User? User { get; set; }
}
