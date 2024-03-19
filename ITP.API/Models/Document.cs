using System;
using System.Collections.Generic;

namespace ITP.API.Models;

public partial class Document
{
    public Guid Id { get; set; }

    public string Path { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<TaskDocument> TaskDocuments { get; set; } = new List<TaskDocument>();
}
