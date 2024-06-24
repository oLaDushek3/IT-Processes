using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ITProcesses.Models;

public partial class Tasks
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProjectId { get; set; }

    public DateTime DateCreateTimestamp { get; set; }

    public DateTime DateStartTimestamp { get; set; }

    public DateTime DateEndTimestamp { get; set; }

    public string Description { get; set; } = null!;

    public int StatusId { get; set; }

    public int TypeId { get; set; }

    public bool Archived { get; set; }

    public Guid? BeforeTask { get; set; }

    public Guid? UserId { get; set; }

    public int CountHour { get; set; }

    public bool? Deleted { get; set; }

    public virtual Tasks? BeforeTaskNavigation { get; set; }

    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    public virtual ICollection<Tasks> InverseBeforeTaskNavigation { get; set; } = new ObservableCollection<Tasks>();

    public virtual Project Project { get; set; } = null!;

    public virtual TaskStatus Status { get; set; } = null!;

    public virtual ICollection<TaskDocument> TaskDocuments { get; set; } = new ObservableCollection<TaskDocument>();

    public virtual ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();

    public virtual Type Type { get; set; } = null!;

    public virtual User? User { get; set; }

    public virtual ICollection<UsersTask> UsersTasks { get; set; } = new ObservableCollection<UsersTask>();
}
