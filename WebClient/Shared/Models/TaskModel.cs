using System;

namespace WebClient.Shared.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Nullable<Guid> Member { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
    }
}
