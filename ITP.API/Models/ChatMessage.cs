using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ITP.API.Models;

public partial class ChatMessage
{
    public int Id { get; set; }

    public Guid? UsersId { get; set; }

    public string? Message { get; set; }

    public Guid? TaskId { get; set; }

    public DateTime? CreatedDate { get; set; }

    [JsonIgnore] public virtual Task? Task { get; set; }

    [JsonIgnore] public virtual User? Users { get; set; }
}