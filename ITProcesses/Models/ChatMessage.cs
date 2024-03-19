using System;
using System.Collections.Generic;

namespace ITProcesses.Models;

public partial class ChatMessage
{
    public int Id { get; set; }

    public Guid? UsersId { get; set; }

    public string? Message { get; set; }

    public Guid? TaskId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Tasks? Task { get; set; }

    public virtual User? Users { get; set; }
}
