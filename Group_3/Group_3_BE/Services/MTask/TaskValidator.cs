using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MTask
{
    public interface ITaskValidator : IServiceScoped
    {
        Task<bool> Create(Entities.Task Task);
        Task<bool> Update(Entities.Task Task);
        Task<bool> Delete(Entities.Task Task);
        Task<bool> BulkDelete(List<Entities.Task> Tasks);
        Task<bool> BulkMerge(List<Entities.Task> Tasks);
        Task<bool> Import(List<Entities.Task> Tasks);
    }
    public class TaskValidator : ITaskValidator
    {
        private IUOW UOW;
        private TaskMessage TaskMessage;

        public TaskValidator(IUOW UOW)
        {
            this.UOW = UOW;
            TaskMessage = new TaskMessage();
        }

        private async Task<bool> ValidateId(Entities.Task Task)
        {
            TaskFilter TaskFilter = new TaskFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Task.Id },
                Selects = TaskSelect.Id,
            };

            int count = await UOW.TaskRepository.Count(TaskFilter);
            if (count == 0)
                Task.AddError(nameof(TaskValidator), nameof(Task.Id), TaskMessage.Error.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(Entities.Task Task)
        {
            if (string.IsNullOrWhiteSpace(Task.Code))
            {
                Task.AddError(nameof(TaskValidator), nameof(Task.Code), TaskMessage.Error.CodeEmpty);
            }
            else if (Task.Code.Length > 50)
            {
                Task.AddError(nameof(TaskValidator), nameof(Task.Code), TaskMessage.Error.CodeOverLength);
            }
            else
            {
                var Code = Task.Code;
                if (Task.Code.Contains(" ") || !Task.Code.ChangeToEnglishChar().Equals(Task.Code))
                {
                    Task.AddError(nameof(TaskValidator), nameof(Task.Code), TaskMessage.Error.CodeHasSpecialCharacter);
                }

                TaskFilter TaskFilter = new TaskFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Task.Id },
                    Code = new StringFilter { Equal = Task.Code },
                    Selects = TaskSelect.Id,
                };

                int count = await UOW.TaskRepository.Count(TaskFilter);
                if (count != 0)
                    Task.AddError(nameof(TaskValidator), nameof(Task.Code), TaskMessage.Error.CodeExisted);
            }
            return Task.IsValidated;
        }

        private async Task<bool> ValidateName(Entities.Task Task)
        {
            if (string.IsNullOrWhiteSpace(Task.Name))
            {
                Task.AddError(nameof(TaskValidator), nameof(Task.Name), TaskMessage.Error.NameEmpty);
            }
            else if (Task.Name.Length > 500)
            {
                Task.AddError(nameof(TaskValidator), nameof(Task.Name), TaskMessage.Error.NameOverLength);
            }
            return Task.IsValidated;
        }

        private async Task<bool> ValidateDescription(Entities.Task Task)
        {
            if (!string.IsNullOrWhiteSpace(Task.Description) && Task.Description.Length > 1000)
            {
                Task.AddError(nameof(TaskValidator), nameof(Task.Description), TaskMessage.Error.DescriptionOverLength);
            }
            return Task.IsValidated;
        }

        private async Task<bool> ValidateStartDate(Entities.Task Task)
        {
            return true;
        }

        private async Task<bool> ValidateFinishDate(Entities.Task Task)
        {
            return true;
        }

        private async Task<bool> ValidatePercentage(Entities.Task Task)
        {
            if (Task.Percentage < 0 || Task.Percentage > 100)
                Task.AddError(nameof(TaskValidator), nameof(Task.Percentage), TaskMessage.Error.PercentageInvalid);
            return Task.IsValidated;
        }

        private async Task<bool> ValidateProject(Entities.Task Task)
        {
            if(Task.ProjectId == 0)
                Task.AddError(nameof(TaskValidator), nameof(Task.Project), TaskMessage.Error.ProjectEmpty);
            else
            {
                ProjectFilter ProjectFilter = new ProjectFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Task.ProjectId },
                    Selects = ProjectSelect.Id,
                };

                int count = await UOW.ProjectRepository.Count(ProjectFilter);
                if (count == 0)
                    Task.AddError(nameof(TaskValidator), nameof(Task.Project), TaskMessage.Error.ProjectNotExisted);               
            }

            return Task.IsValidated;
        }

        private async Task<bool> ValidateTaskType(Entities.Task Task)
        {
            if (Task.TaskTypeId == 0)
                Task.AddError(nameof(TaskValidator), nameof(Task.TaskType), TaskMessage.Error.TaskTypeEmpty);
            else
            {
                TaskTypeFilter TaskTypeFilter = new TaskTypeFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Task.TaskTypeId },
                    Selects = TaskTypeSelect.Id,
                };

                int count = await UOW.TaskTypeRepository.Count(TaskTypeFilter);
                if (count == 0)
                    Task.AddError(nameof(TaskValidator), nameof(Task.TaskType), TaskMessage.Error.TaskTypeNotExisted);
            }
            return Task.IsValidated;
        }

        private async Task<bool> ValidateStatus(Entities.Task Task)
        {
            if (StatusEnum.ACTIVE.Id != Task.Id && StatusEnum.INACTIVE.Id != Task.Id)
                Task.AddError(nameof(TaskValidator), nameof(Task.Description), TaskMessage.Error.StatusNotExisted);
            return Task.IsValidated;
        }

        public async Task<bool> Create(Entities.Task Task)
        {
            await ValidateCode(Task);
            await ValidateName(Task);
            await ValidateDescription(Task);
            await ValidateStartDate(Task);
            await ValidateFinishDate(Task);
            await ValidatePercentage(Task);
            await ValidateProject(Task);
            await ValidateTaskType(Task);
            await ValidateStatus(Task);

            return Task.IsValidated;
        }

        public async Task<bool> Update(Entities.Task Task)
        {
            if(await ValidateId(Task))
            {
                await ValidateCode(Task);
                await ValidateName(Task);
                await ValidateDescription(Task);
                await ValidateStartDate(Task);
                await ValidateFinishDate(Task);
                await ValidatePercentage(Task);
                await ValidateProject(Task);
                await ValidateTaskType(Task);
                await ValidateStatus(Task);
            }
            return Task.IsValidated;
        }

        public async Task<bool> Delete(Entities.Task Task)
        {
            if(await ValidateId(Task))
            {
                var oldData = await UOW.TaskRepository.Get(Task.Id);
                if (oldData.Used == true)
                {
                    Task.AddError(nameof(TaskValidator), nameof(Task.Id), TaskMessage.Error.TaskInUsed);
                }
            }
            return Task.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Entities.Task> Tasks)
        {
            foreach (var Task in Tasks)
            {
                await Delete(Task);
            }
            return Tasks.All(x => x.IsValidated);
        }

        public async Task<bool> BulkMerge(List<Entities.Task> Tasks)
        {
            return true;
        }

        public async Task<bool> Import(List<Entities.Task> Tasks)
        {
            foreach (var Task in Tasks)
            {
                await Create(Task);
            }
            return Tasks.All(x => x.IsValidated);
        }
    }
}
