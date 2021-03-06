﻿using Domain.Commands;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Services
{
    public interface ITaskService
    {
        Task<GetAllTasksQueryResult> GetAllTasksQueryHandler();
        Task<CreateTaskCommandResult> CreateTaskCommandHandler(CreateTaskCommand command);
        Task<UpdateMemberCommandResult> UpdateTaskCommandHandler(CreateTaskCommand command);
    }
}
