using Group_3_BE.Entities;
using Group_3_BE.Helpers;
using Group_3_BE.Models;
using Group_3_BE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MTaskType
{
    public interface ITaskTypeService : IServiceScoped
    {
        Task<int> Count(TaskTypeFilter TaskTypeFilter);
        Task<List<TaskType>> List(TaskTypeFilter TaskTypeFilter);
        Task<TaskType> Get(long Id);
        Task<TaskType> Create(TaskType TaskType);
        Task<TaskType> Update(TaskType TaskType);
        Task<TaskType> Delete(TaskType TaskType);
        Task<List<TaskType>> BulkDelete(List<TaskType> TaskTypes);
        Task<List<TaskType>> BulkMerge(List<TaskType> TaskTypes);
        Task<List<TaskType>> Import(List<TaskType> TaskTypes);
        Task<TaskTypeFilter> ToFilter(TaskTypeFilter TaskTypeFilter);

    }
    public class TaskTypeService : ITaskTypeService
    {
        private IUOW UOW;
        private ITaskTypeValidator TaskTypeValidator;
        private ILogging Logging;

        public TaskTypeService
        (
            IUOW UOW,
            ITaskTypeValidator TaskTypeValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.TaskTypeValidator = TaskTypeValidator;
            this.Logging = Logging;
        }

        public async Task<int> Count(TaskTypeFilter TaskTypeFilter)
        {
            try
            {
                int result = await UOW.TaskTypeRepository.Count(TaskTypeFilter);
                return result;
            }
            catch(Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskTypeService));
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public async Task<List<TaskType>> List(TaskTypeFilter TaskTypeFilter)
        {
            try
            {
                List<TaskType> TaskTypes = await UOW.TaskTypeRepository.List(TaskTypeFilter); ;
                return TaskTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskTypeService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<TaskType> Get(long Id)
        {
            TaskType TaskType = await UOW.TaskTypeRepository.Get(Id);
            if (TaskType == null)
                return null;
            return TaskType;
        }

        public async Task<TaskType> Create(TaskType TaskType)
        {
            if (!await TaskTypeValidator.Create(TaskType))
                return TaskType;
            try
            {
                await UOW.TaskTypeRepository.Create(TaskType);
                TaskType = await UOW.TaskTypeRepository.Get(TaskType.Id);
                Logging.CreateAuditLog(TaskType, new { }, nameof(TaskTypeService));
                return TaskType;
            }
            catch(Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskTypeService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<TaskType> Update(TaskType TaskType)
        {
            if (!await TaskTypeValidator.Update(TaskType))
                return TaskType;
            try
            {
                var oldData = await UOW.TaskTypeRepository.Get(TaskType.Id);

                await UOW.TaskTypeRepository.Update(TaskType);

                TaskType = await UOW.TaskTypeRepository.Get(TaskType.Id);
                Logging.CreateAuditLog(TaskType, oldData, nameof(TaskTypeService));
                return TaskType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskTypeService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<TaskType> Delete(TaskType TaskType)
        {
            if (!await TaskTypeValidator.Delete(TaskType))
                return TaskType;
            try
            {
                await UOW.TaskTypeRepository.Delete(TaskType);
                TaskType = (await UOW.TaskTypeRepository.List(new List<long> { TaskType.Id})).FirstOrDefault();
                Logging.CreateAuditLog(new { }, TaskType, nameof(TaskTypeService));
                return TaskType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskTypeService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<TaskType>> BulkDelete(List<TaskType> TaskTypes)
        {
            if (!await TaskTypeValidator.BulkDelete(TaskTypes))
                return TaskTypes;

            try
            {
                await UOW.TaskTypeRepository.BulkDelete(TaskTypes);
                var Ids = TaskTypes.Select(x => x.Id).ToList();
                TaskTypes = await UOW.TaskTypeRepository.List(Ids);
                Logging.CreateAuditLog(new { }, TaskTypes, nameof(TaskTypeService));
                return TaskTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskTypeService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<TaskType>> BulkMerge(List<TaskType> TaskTypes)
        {
            if (!await TaskTypeValidator.BulkMerge(TaskTypes))
                return TaskTypes;
            try
            {
                await UOW.TaskTypeRepository.BulkMerge(TaskTypes);
                var Ids = TaskTypes.Select(x => x.Id).ToList();
                TaskTypes = await UOW.TaskTypeRepository.List(Ids);
                Logging.CreateAuditLog(TaskTypes, new { }, nameof(TaskTypeService));
                return TaskTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskTypeService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<TaskType>> Import(List<TaskType> TaskTypes)
        {
            if (!await TaskTypeValidator.Import(TaskTypes))
                return TaskTypes;
            try
            {
                await UOW.TaskTypeRepository.BulkMerge(TaskTypes);
                var Ids = TaskTypes.Select(x => x.Id).ToList();
                TaskTypes = await UOW.TaskTypeRepository.List(Ids);
                Logging.CreateAuditLog(TaskTypes, new { }, nameof(TaskTypeService));
                return TaskTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaskTypeService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<TaskTypeFilter> ToFilter(TaskTypeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<TaskTypeFilter>();
            return filter;
        }

    }
}
