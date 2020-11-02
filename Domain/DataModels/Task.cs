﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataModels
{
    public class Task
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Subject { get; set; }
        public Boolean IsComplete { get; set; }
        public Member Member { get; set; }
        public Nullable<Guid> AssignedToId { get; set; }
    }
}
