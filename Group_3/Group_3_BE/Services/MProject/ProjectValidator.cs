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

    }
}
