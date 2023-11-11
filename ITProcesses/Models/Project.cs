using System;
using System.Collections.Generic;

namespace ITProcesses.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool? Archived { get; set; }

    public Guid? UserId { get; set; }

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();

    public virtual User? User { get; set; }
}
