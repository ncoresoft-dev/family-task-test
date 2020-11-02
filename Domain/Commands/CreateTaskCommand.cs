using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CreateTaskCommand
    {
        public Guid Id { get; set; }
        public Nullable<Guid> Member { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
    }
}
