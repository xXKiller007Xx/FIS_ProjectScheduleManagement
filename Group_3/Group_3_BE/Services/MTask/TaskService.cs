using Group_3_BE.Entities;
using Group_3_BE.Helpers;
using Group_3_BE.Models;
using Group_3_BE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MTask
{
    public interface ITaskService : IServiceScoped
    {
        Task<int> Count(TaskFilter TaskFilter);
        Task<List<Entities.Task>> List(TaskFilter TaskFilter);
        Task<Entities.Task> Get(long Id);
        Task<Entities.Task> Create(Entities.Task Task);
        Task<Entities.Task> Update(Entities.Task Task);
        Task<Entities.Task> Delete(Entities.Task Task);
        Task<List<Entities.Task>> BulkDelete(List<Entities.Task> Tasks);
        Task<List<Entities.Task>> BulkMerge(List<Entities.Task> Tasks);
        Task<List<Entities.Task>> Import(List<Entities.Task> Tasks);
        Task<TaskFilter> ToFilter(TaskFilter TaskFilter);
    }

    public class TaskService : ITaskService
    {
        private IUOW UOW;
        private ITaskValidator TaskValidator;
        private ILogging Logging;

        public TaskService
        (
            IUOW UOW,
            ITaskValidator TaskValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.TaskValidator = TaskValidator;
            this.Logging = Logging;
        }

        public async Task<int> Count(TaskFilter TaskFilter)
        {
            try
            {
                int result = await UOW.TaskRepository.Count(TaskFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskService));
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public async Task<List<Entities.Task>> List(TaskFilter TaskFilter)
        {
            try
            {
                List<Entities.Task> Tasks = await UOW.TaskRepository.List(TaskFilter);
                return Tasks;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<Entities.Task> Get(long Id)
        {
            Entities.Task Task = await UOW.TaskRepository.Get(Id);
            if (Task == null)
                return null;
            return Task;
        }

        public async Task<Entities.Task> Create(Entities.Task Task)
        {
            if (!await TaskValidator.Create(Task))
                return Task;

            try
            {
                await UOW.TaskRepository.Create(Task);
                Task = await UOW.TaskRepository.Get(Task.Id);
                Logging.CreateAuditLog(Task, new { }, nameof(TaskService));
                return Task;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<Entities.Task> Update(Entities.Task Task)
        {
            if (!await TaskValidator.Update(Task))
                return Task;

            try
            {
                var oldData = await UOW.TaskRepository.Get(Task.Id);

                await UOW.TaskRepository.Update(Task);

                Task = await UOW.TaskRepository.Get(Task.Id);
                Logging.CreateAuditLog(Task, oldData, nameof(TaskService));
                return Task;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<Entities.Task> Delete(Entities.Task Task)
        {
            if (await TaskValidator.Delete(Task))
                return Task;

            try
            {
                await UOW.TaskRepository.Delete(Task);
                Task = (await UOW.TaskRepository.List(new List<long> { Task.Id })).FirstOrDefault();
                Logging.CreateAuditLog(new { }, Task, nameof(TaskService));
                return Task;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<Entities.Task>> BulkDelete(List<Entities.Task> Tasks)
        {
            if (await TaskValidator.BulkDelete(Tasks))
                return Tasks;

            try
            {
                await UOW.TaskRepository.BulkDelete(Tasks);
                var Ids = Tasks.Select(x => x.Id).ToList();
                Tasks = await UOW.TaskRepository.List(Ids);

                Logging.CreateAuditLog(new { }, Tasks, nameof(TaskService));
                return Tasks;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        
        public async Task<List<Entities.Task>> BulkMerge(List<Entities.Task> Tasks)
        {
            if (await TaskValidator.BulkMerge(Tasks))
                return Tasks;

            try
            {
                await UOW.TaskRepository.BulkMerge(Tasks);
                var Ids = Tasks.Select(x => x.Id).ToList();
                Tasks = await UOW.TaskRepository.List(Ids);

                Logging.CreateAuditLog(Tasks, new { }, nameof(TaskService));
                return Tasks;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        
        public async Task<List<Entities.Task>> Import(List<Entities.Task> Tasks)
        {
            if (await TaskValidator.Import(Tasks))
                return Tasks;

            try
            {
                await UOW.TaskRepository.BulkMerge(Tasks);
                var Ids = Tasks.Select(x => x.Id).ToList();
                Tasks = await UOW.TaskRepository.List(Ids);

                Logging.CreateAuditLog(Tasks, new { }, nameof(TaskService));
                return Tasks;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        
        public async Task<TaskFilter> ToFilter(TaskFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<TaskFilter>();
            return filter;
        }
    }
}
