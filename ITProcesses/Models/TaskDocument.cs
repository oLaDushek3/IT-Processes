﻿using System;
using System.Collections.Generic;

namespace ITProcesses.Models;

public partial class TaskDocument
{
    public int Id { get; set; }

    public Guid? TaskId { get; set; }

    public Guid? DocumentsId { get; set; }

    public virtual Document? Documents { get; set; }

    public virtual Tasks? Task { get; set; }
}
