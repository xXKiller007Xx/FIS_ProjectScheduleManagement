using Group_3_BE.Entities;
using Group_3_BE.Helpers;
using Group_3_BE.Models;
using Group_3_BE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MProject
{
    public interface IProjectService : IServiceScoped
    {
        Task<int> Count(ProjectFilter ProjectFilter);
        Task<List<Project>> List(ProjectFilter ProjectFilter);
        Task<Project> Get(long Id);
        Task<Project> Create(Project Project);
        Task<Project> Update(Project Project);
        Task<Project> Delete(Project Project);
        Task<List<Project>> BulkDelete(List<Project> Projects);
        Task<List<Project>> BulkMerge(List<Project> Projects);
        Task<List<Project>> Import(List<Project> Projects);
        Task<ProjectFilter> ToFilter(ProjectFilter ProjectFilter);
    }

    public class ProjectService : IProjectService
    {
        private IUOW UOW;
        private IProjectValidator ProjectValidator;
        private ILogging Logging;

        public ProjectService
        (
            IUOW UOW,
            IProjectValidator ProjectValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.ProjectValidator = ProjectValidator;
            this.Logging = Logging;
        }

        public async Task<int> Count(ProjectFilter ProjectFilter)
        {
            try
            {
                int result = await UOW.ProjectRepository.Count(ProjectFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProjectService));
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public async Task<List<Project>> List(ProjectFilter ProjectFilter)
        {
            try
            {
                List<Project> Projects = await UOW.ProjectRepository.List(ProjectFilter);
                return Projects;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProjectService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<Project> Get(long Id)
        {
            Project Project = await UOW.ProjectRepository.Get(Id);
            if (Project == null)
                return null;
            return Project;
        }

        public async Task<Project> Create(Project Project)
        {
            if (!await ProjectValidator.Create(Project))
                return Project;

            try
            {
                await UOW.ProjectRepository.Create(Project);
                Project = await UOW.ProjectRepository.Get(Project.Id);
                Logging.CreateAuditLog(Project, new { }, nameof(ProjectService));
                return Project;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProjectService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<Project> Update(Project Project)
        {
            if (!await ProjectValidator.Update(Project))
                return Project;

            try
            {
                var oldData = await UOW.ProjectRepository.Get(Project.Id);

                await UOW.ProjectRepository.Update(Project);

                Project = await UOW.ProjectRepository.Get(Project.Id);
                Logging.CreateAuditLog(Project, oldData, nameof(ProjectService));
                return Project;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProjectService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<Project> Delete(Project Project)
        {
            if (!await ProjectValidator.Delete(Project))
                return Project;
            try
            {
                await UOW.ProjectRepository.Delete(Project);
                Project = (await UOW.ProjectRepository.List(new List<long> { Project.Id })).FirstOrDefault();
                Logging.CreateAuditLog(new { }, Project, nameof(ProjectService));
                return Project;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProjectService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<Project>> BulkDelete(List<Project> Projects)
        {
            if (!await ProjectValidator.BulkDelete(Projects))
                return Projects;

            try
            {
                await UOW.ProjectRepository.BulkDelete(Projects);
                var Ids = Projects.Select(x => x.Id).ToList();
                Projects = await UOW.ProjectRepository.List(Ids);
                Logging.CreateAuditLog(new { }, Projects, nameof(ProjectService));
                return Projects;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProjectService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<Project>> BulkMerge(List<Project> Projects)
        {
            if (!await ProjectValidator.BulkMerge(Projects))
                return Projects;

            try
            {
                await UOW.ProjectRepository.BulkMerge(Projects);
                var Ids = Projects.Select(x => x.Id).ToList();
                Projects = await UOW.ProjectRepository.List(Ids);
                Logging.CreateAuditLog(Projects, new { }, nameof(ProjectService));
                return Projects;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProjectService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<Project>> Import(List<Project> Projects)
        {
            if (!await ProjectValidator.Import(Projects))
                return Projects;

            try
            {
                await UOW.ProjectRepository.BulkMerge(Projects);
                var Ids = Projects.Select(x => x.Id).ToList();
                Projects = await UOW.ProjectRepository.List(Ids);
                Logging.CreateAuditLog(Projects, new { }, nameof(ProjectService));
                return Projects;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProjectService));
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<ProjectFilter> ToFilter(ProjectFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ProjectFilter>();
            return filter;
        }
    }
}
