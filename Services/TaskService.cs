using AutoMapper;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.DataModels;
using Domain.Queries;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(IMapper mapper, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }

        public async Task<GetAllTasksQueryResult> GetAllTasksQueryHandler()
        {
            try
            {
                IEnumerable<TaskVm> vm = new List<TaskVm>();
                List<TaskVm> vmlist = new List<TaskVm>();
                var tasks = await _taskRepository.Reset().ToListAsync();

                if (tasks != null && tasks.Any())
                {
                    //vm = _mapper.Map<IEnumerable<TaskVm>>(tasks);
                    foreach (var item in tasks)
                    {
                        TaskVm taskVm = new TaskVm
                        {
                            Id = item.Id,
                            AssignedToId = item.AssignedToId,
                            IsComplete = item.IsComplete,
                            Subject = item.Subject,
                        };
                        vmlist.Add(taskVm);
                    }
                    vm = new List<TaskVm>();
                    vm = vmlist.AsEnumerable();
                }

                return new GetAllTasksQueryResult()
                {
                    Payload = vm
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CreateTaskCommandResult> CreateTaskCommandHandler(CreateTaskCommand command)
        {
            try
            {
                Domain.DataModels.Task task = new Domain.DataModels.Task
                {
                    AssignedToId = command.Member,
                    IsComplete = command.IsDone,
                    Subject = command.Text
                };
                //var task = _mapper.Map<Domain.DataModels.Task>(command);
                var createdTask = await _taskRepository.CreateRecordAsync(task);
                TaskVm vm = new TaskVm();
                if (createdTask != null)
                {
                    vm.Id = createdTask.Id;
                    vm.Subject = createdTask.Subject;
                    vm.IsComplete = createdTask.IsComplete;
                    vm.AssignedToId = createdTask.AssignedToId;
                }
                //var vm = _mapper.Map<TaskVm>(persistedMember);

                return new CreateTaskCommandResult()
                {
                    Payload = vm
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<UpdateMemberCommandResult> UpdateTaskCommandHandler(CreateTaskCommand command)
        {
            var isSucceed = true;
            Domain.DataModels.Task task = new Domain.DataModels.Task();
            task = await _taskRepository.ByIdAsync(command.Id);
            //_mapper.Map<CreateTaskCommand, Domain.DataModels.Task>(command, task);
            task.AssignedToId = command.Member;
            task.IsComplete = command.IsDone;
            task.Subject = command.Text;
            if (task.Id != null)
            {
                var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task);

                if (affectedRecordsCount < 1)
                    isSucceed = false;
            }

            return new UpdateMemberCommandResult()
            {
                Succeed = isSucceed
            };
        }
    }
}
