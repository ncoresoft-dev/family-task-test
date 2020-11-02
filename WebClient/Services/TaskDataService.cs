using Domain.Commands;
using Domain.Queries;
using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Abstractions;
using WebClient.Shared.Models;

namespace WebClient.Services
{
    public class TaskDataService : ITaskDataService
    {
        private readonly HttpClient httpClient;
        public TaskDataService(IHttpClientFactory clientFactory)
        {
            httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            tasks = new List<TaskVm>();
            LoadTasks();
        }
        private IEnumerable<TaskVm> tasks;

        public IEnumerable<TaskVm> Task => tasks;
        public event EventHandler TasksChanged;

        public async void LoadTasks()
        {
            try
            {
                tasks = (await GetAllTasks()).Payload;
                Tasks = tasks.ToList();
                //List<TaskModel> list = new List<TaskModel>();
                //if (tasks.ToList().Count > 0)
                //{
                //    foreach (var item in tasks.ToList())
                //    {
                //        TaskModel taskModel = new TaskModel
                //        {
                //            Id = item.Id,
                //            IsDone = item.IsComplete,
                //            Member = item.AssignedToId != null ? (Guid)item.AssignedToId : Guid.Empty,
                //            Text = item.Subject
                //        };
                //        list.Add(taskModel);
                //    }
                //    Tasks = list;
                //}
                TasksChanged?.Invoke(this, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<GetAllTasksQueryResult> GetAllTasks()
        {
            return await httpClient.GetJsonAsync<GetAllTasksQueryResult>("tasks");
        }

        public List<TaskVm> Tasks { get; private set; }
        public TaskVm SelectedTask { get; private set; }


        public event EventHandler TasksUpdated;
        public event EventHandler TaskSelected;

        public void SelectTask(Guid id)
        {
            SelectedTask = Tasks.SingleOrDefault(t => t.Id == id);
            TasksUpdated?.Invoke(this, null);
        }

        public async void ToggleTask(Guid id)
        {
            foreach (var taskModel in Tasks)
            {
                if (taskModel.Id == id)
                {
                    CreateTaskCommand command = new CreateTaskCommand
                    {
                        Id = taskModel.Id,
                        IsDone = !taskModel.IsComplete,
                        Member = taskModel.AssignedToId,
                        Text = taskModel.Subject,
                    };
                    await Update(command);
                    taskModel.IsComplete = !taskModel.IsComplete;
                }
            }

            TasksUpdated?.Invoke(this, null);
        }

        public async Task<Boolean> AddTask(TaskModel model)
        {
            Boolean flag = true;
            if (model != null)
            {
                CreateTaskCommand createTask = new CreateTaskCommand
                {
                    IsDone = model.IsDone,
                    Member = model.Member,
                    Text = model.Text
                };
                var result = await Create(createTask);
                if (result != null)
                {
                    var updatedList = (await GetAllTasks()).Payload;

                    if (updatedList != null)
                    {
                        Tasks = updatedList.ToList();
                        //tasks = updatedList;
                        //List<TaskModel> list = new List<TaskModel>();
                        //foreach (var item in tasks.ToList())
                        //{
                        //    TaskModel taskModel = new TaskModel
                        //    {
                        //        Id = item.Id,
                        //        IsDone = item.IsComplete,
                        //        Member = item.AssignedToId != null ? (Guid)item.AssignedToId : Guid.Empty,
                        //        Text = item.Subject
                        //    };
                        //    list.Add(taskModel);
                        //}
                        //Tasks = list;
                        TasksChanged?.Invoke(this, null);

                    }
                    else
                        flag = false;
                }
                else
                    flag = false;
            }
            else
                flag = false;
            return flag;
        }

        public async Task<Boolean> AssignTask(Domain.ClientSideModels.MenuItem member,TaskVm task)
        {
            Boolean flag = true;
            CreateTaskCommand command = new CreateTaskCommand
            {
                Id = task.Id,
                IsDone = task.IsComplete,
                Member = member.referenceId,
                Text = task.Subject,
            };
            await Update(command);
            var updatedList = (await GetAllTasks()).Payload;

            if (updatedList != null)
            {
                Tasks = updatedList.ToList();
                TasksChanged?.Invoke(this, null);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        private async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {
            return await httpClient.PostJsonAsync<CreateTaskCommandResult>("tasks", command);
        }

        private async Task<UpdateMemberCommandResult> Update(CreateTaskCommand command)
        {
            return await httpClient.PutJsonAsync<UpdateMemberCommandResult>($"tasks/{command.Id}", command);
        }
    }
}