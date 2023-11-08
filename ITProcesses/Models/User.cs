using System;
using System.Collections.Generic;

namespace ITProcesses.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public string? Image { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<UsersTask> UsersTasks { get; set; } = new List<UsersTask>();
}
