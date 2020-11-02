using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Services
{
    public class StateContainer
    {
        public TaskVm Task { get; set; }

        public event Action OnChange;

        public void SetTask(TaskVm value)
        {
            Task = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
