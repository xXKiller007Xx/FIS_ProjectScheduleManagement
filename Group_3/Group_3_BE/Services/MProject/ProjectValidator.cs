using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MProject
{
    public interface IProjectValidator : IServiceScoped
    {
        Task<bool> Create(Project Project);
        Task<bool> Update(Project Project);
        Task<bool> Delete(Project Project);
        Task<bool> BulkDelete(List<Project> Projects);
        Task<bool> BulkMerge(List<Project> TaskTypes);
        Task<bool> Import(List<Project> Projects);
    }

    public class ProjectValidator : IProjectValidator
    {
        private IUOW UOW;
        private ProjectMessage ProjectMessage;

        public ProjectValidator(IUOW UOW)
        {
            this.UOW = UOW;
            this.ProjectMessage = new ProjectMessage();
        }

        private async Task<bool> ValidateId(Project Project)
        {
            ProjectFilter ProjectFilter = new ProjectFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Project.Id },
                Selects = ProjectSelect.Id,
            };

            int count = await UOW.ProjectRepository.Count(ProjectFilter);
            if (count == 0)
                Project.AddError(nameof(ProjectValidator), nameof(Project.Id), ProjectMessage.Error.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(Project Project)
        {
            if (string.IsNullOrWhiteSpace(Project.Code))
            {
                Project.AddError(nameof(ProjectValidator), nameof(Project.Code), ProjectMessage.Error.CodeEmpty);
            }
            else if (Project.Code.Length > 50)
            {
                Project.AddError(nameof(ProjectValidator), nameof(Project.Code), ProjectMessage.Error.CodeOverLength);
            }
            else
            {
                var Code = Project.Code;
                if (Project.Code.Contains(" ") || !Project.Code.ChangeToEnglishChar().Equals(Project.Code))
                {
                    Project.AddError(nameof(ProjectValidator), nameof(Project.Code), ProjectMessage.Error.CodeHasSpecialCharacter);
                }

                ProjectFilter ProjectFilter = new ProjectFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Project.Id },
                    Code = new StringFilter { Equal = Project.Code },
                    Selects = ProjectSelect.Id,
                };

                int count = await UOW.ProjectRepository.Count(ProjectFilter);
                if (count != 0)
                    Project.AddError(nameof(ProjectValidator), nameof(Project.Code), ProjectMessage.Error.CodeExisted);
            }
            return Project.IsValidated;
        }

        private async Task<bool> ValidateName(Project Project)
        {
            if (string.IsNullOrWhiteSpace(Project.Name))
            {
                Project.AddError(nameof(ProjectValidator), nameof(Project.Name), ProjectMessage.Error.NameEmpty);
            }
            else if (Project.Name.Length > 500)
            {
                Project.AddError(nameof(ProjectValidator), nameof(Project.Name), ProjectMessage.Error.NameOverLength);
            }
            return Project.IsValidated;
        }

        private async Task<bool> ValidateDescription(Project Project)
        {
            if (!string.IsNullOrWhiteSpace(Project.Description) && Project.Description.Length > 1000)
            {
                Project.AddError(nameof(ProjectValidator), nameof(Project.Description), ProjectMessage.Error.DescriptionOverLength);
            }
            return Project.IsValidated;
        }

        private async Task<bool> ValidateStartDate(Project Project)
        {
            return true;
        }

        private async Task<bool> ValidateFinishDate(Project Project)
        {
            return true;
        }

        private async Task<bool> ValidatePercentage(Project Project)
        {
            if(Project.Percentage < 0 || Project.Percentage > 100)
                Project.AddError(nameof(ProjectValidator), nameof(Project.Percentage), ProjectMessage.Error.PercentageInvalid);
            return Project.IsValidated;
        }

        private async Task<bool> ValidateStatus(Project Project)
        {
            if (StatusEnum.ACTIVE.Id != Project.Id && StatusEnum.INACTIVE.Id != Project.Id)
                Project.AddError(nameof(ProjectValidator), nameof(Project.Description), ProjectMessage.Error.StatusNotExisted);
            return Project.IsValidated;
        }

        public async Task<bool> Create(Project Project)
        {
            await ValidateCode(Project);
            await ValidateName(Project);
            await ValidateDescription(Project);
            await ValidateStartDate(Project);
            await ValidateFinishDate(Project);
            await ValidatePercentage(Project);
            await ValidateStatus(Project);

            return Project.IsValidated;
        }

        public async Task<bool> Update(Project Project)
        {
            if(await ValidateId(Project))
            {
                await ValidateCode(Project);
                await ValidateName(Project);
                await ValidateDescription(Project);
                await ValidateStartDate(Project);
                await ValidateFinishDate(Project);
                await ValidatePercentage(Project);
                await ValidateStatus(Project);
            }
            return Project.IsValidated;
        }

        public async Task<bool> Delete(Project Project)
        {
            if (await ValidateId(Project))
            {
                var oldData = await UOW.ProjectRepository.Get(Project.Id);
                if (oldData.Used != null && oldData.Used == true)
                {
                    Project.AddError(nameof(ProjectValidator), nameof(Project.Id), ProjectMessage.Error.ProjectInUsed);
                }
            }
            return Project.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Project> Projects)
        {
            foreach (var Project in Projects)
            {
                await Delete(Project);
            }
            return Projects.All(x => x.IsValidated);
        }

        public async Task<bool> BulkMerge(List<Project> TaskTypes)
        {
            return true;
        }

        public async Task<bool> Import(List<Project> Projects)
        {
            foreach (Project Project in Projects)
            {
                await Create(Project);
            }
            return Projects.Any(x => x.IsValidated) ? false : true;
        }

    }
}
