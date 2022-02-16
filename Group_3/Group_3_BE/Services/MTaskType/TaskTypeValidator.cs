using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MTaskType
{
    public interface ITaskTypeValidator : IServiceScoped
    {
        System.Threading.Tasks.Task Get(TaskType TaskType);
        Task<bool> Create(TaskType TaskType);
        Task<bool> Update(TaskType TaskType);
        Task<bool> Delete(TaskType TaskType);
        Task<bool> BulkDelete(List<TaskType> TaskType);
        Task<bool> BulkMerge(List<TaskType> TaskType);
        Task<bool> Import(List<TaskType> TaskType);
    }

    public class TaskTypeValidator : ITaskTypeValidator 
    {
        private IUOW UOW;
        private TaskTypeMessage TaskTypeMessage;

        public TaskTypeValidator(IUOW UOW)
        {
            this.UOW = UOW;
            this.TaskTypeMessage = new TaskTypeMessage();
        }

        private async Task<bool> ValidateId(TaskType TaskType)
        {
            TaskTypeFilter TaskTypeFilter = new TaskTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = TaskType.Id },
                Selects = TaskTypeSelect.Id,
            };

            int count = await UOW.TaskTypeRepository.Count(TaskTypeFilter);
            if (count == 0)
                TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Id), TaskTypeMessage.Error.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(TaskType TaskType)
        {
            if(string.IsNullOrWhiteSpace(TaskType.Code))
            {
                TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Code), TaskTypeMessage.Error.CodeEmpty);
            }
            else if(TaskType.Code.Length > 50)
            {
                TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Code), TaskTypeMessage.Error.CodeOverLength);
            }
            else
            {
                var Code = TaskType.Code;
                if(TaskType.Code.Contains(" ") || !TaskType.Code.ChangeToEnglishChar().Equals(TaskType.Code))
                {
                    TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Code), TaskTypeMessage.Error.CodeHasSpecialCharacter);
                }

                TaskTypeFilter TaskTypeFilter = new TaskTypeFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = TaskType.Id },
                    Code = new StringFilter { Equal = TaskType.Code },
                    Selects = TaskTypeSelect.Id,
                };

                int count = await UOW.TaskTypeRepository.Count(TaskTypeFilter);
                if (count != 0)
                    TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Code), TaskTypeMessage.Error.CodeExisted);
            }
            return TaskType.IsValidated;
        }

        private async Task<bool> ValidateName(TaskType TaskType)
        {
            if(string.IsNullOrWhiteSpace(TaskType.Name))
            {
                TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Name), TaskTypeMessage.Error.NameEmpty);
            }
            else if(TaskType.Name.Length > 500)
            {
                TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Name), TaskTypeMessage.Error.NameOverLength);
            }
            return TaskType.IsValidated;
        }

        private async Task<bool> ValidateDescription(TaskType TaskType)
        {
            if(!string.IsNullOrWhiteSpace(TaskType.Description) && TaskType.Description.Length > 1000)
            {
                TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Description), TaskTypeMessage.Error.DescriptionOverLength);
            }
            return TaskType.IsValidated;
        }

        private async System.Threading.Tasks.Task<bool> ValidateStatus(TaskType TaskType)
        {
            if(StatusEnum.ACTIVE.Id != TaskType.Id && StatusEnum.INACTIVE.Id != TaskType.Id)
                TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Description), TaskTypeMessage.Error.StatusNotExisted);
            return TaskType.IsValidated;
        }


        public async System.Threading.Tasks.Task Get(TaskType TaskType)
        {

        }

        public async Task<bool> Create(TaskType TaskType)
        {
            await ValidateCode(TaskType);
            await ValidateName(TaskType);
            await ValidateDescription(TaskType);
            await ValidateStatus(TaskType);
            return TaskType.IsValidated;
        }

        public async Task<bool> Update(TaskType TaskType)
        {
            if(await ValidateId(TaskType))
            {
                await ValidateCode(TaskType);
                await ValidateName(TaskType);
                await ValidateDescription(TaskType);
                await ValidateStatus(TaskType);
            }
            return TaskType.IsValidated;
        }

        public async Task<bool> Delete(TaskType TaskType)
        {
            if (await ValidateId(TaskType))
            {
                var oldData = await UOW.TaskTypeRepository.Get(TaskType.Id);
                //if(oldData.Used)
                //{
                //    TaskType.AddError(nameof(TaskTypeValidator), nameof(TaskType.Id), TaskTypeMessage.Error.TaskTypeInUsed);
                //}
            }
            return TaskType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<TaskType> TaskTypes)
        {
            foreach(var TaskType in TaskTypes)
            {
                await Delete(TaskType);
            }
            return TaskTypes.All(x => x.IsValidated);
        }

        public async Task<bool> BulkMerge(List<TaskType> TaskTypes)
        {
            //foreach (var TaskType in TaskTypes)
            //{
            //    await Update(TaskType);
            //}
            //return TaskTypes.All(x => x.IsValidated);
            return true;
        }

        public async Task<bool> Import(List<TaskType> TaskTypes)
        {
            foreach(TaskType TaskType in TaskTypes)
            {
                await Create(TaskType);
            }
            return TaskTypes.Any(x => x.IsValidated) ? false : true;
        }
    }
}
