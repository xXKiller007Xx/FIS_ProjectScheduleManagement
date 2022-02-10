using Group_3_BE.Entities;
using Group_3_BE.Helpers;
using Group_3_BE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Repositories
{
    public interface IProjectRepository
    {
        Task<int> Count(ProjectFilter ProjectFilter);
        Task<List<Project>> List(ProjectFilter ProjectFilter);
        Task<List<Project>> List(List<long> Ids);
        Task<Project> Get(long Id);
        Task<bool> Create(Project Project);
        Task<bool> Update(Project Project);
        Task<bool> Delete(Project Project);
        Task<bool> BulkMerge(List<Project> Projects);
        Task<bool> BulkUpdate(List<Project> Projects);
        Task<bool> BulkDelete(List<Project> Projects);
        Task<bool> Used(List<long> Ids);
    }
    public class ProjectRepository : IProjectRepository
    {
        private DataContext DataContext;
        public ProjectRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }
        private IQueryable<ProjectDAO> DynamicFilter(IQueryable<ProjectDAO> query, ProjectFilter filter)
        {
            if (filter == null)
                return query.Where(query => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.StartDate, filter.StartDate);
            query = query.Where(q => q.FinishDate, filter.FinishDate);
            query = query.Where(q => q.Percentage, filter.Percentage);
            query = OrFilter(query, filter);
            return query;

        }
        private IQueryable<ProjectDAO> OrFilter(IQueryable<ProjectDAO> query, ProjectFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProjectDAO> initQuery = query.Where(q => false);
            foreach (ProjectFilter ProjectFilter in filter.OrFilter)
            {
                IQueryable<ProjectDAO> queryable = query;
                query = query.Where(q => q.Id, filter.Id);
                query = query.Where(q => q.Code, filter.Code);
                query = query.Where(q => q.Name, filter.Name);
                query = query.Where(q => q.StatusId, filter.StatusId);
                query = query.Where(q => q.Description, filter.Description);
                query = query.Where(q => q.StartDate, filter.StartDate);
                query = query.Where(q => q.FinishDate, filter.FinishDate);
                query = query.Where(q => q.Percentage, filter.Percentage);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }
        private IQueryable<ProjectDAO> DynamicOrder(IQueryable<ProjectDAO> query, ProjectFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProjectOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProjectOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProjectOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ProjectOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case ProjectOrder.StartDate:
                            query = query.OrderBy(q => q.StartDate);
                            break;
                        case ProjectOrder.FinishDate:
                            query = query.OrderBy(q => q.FinishDate);
                            break;
                        case ProjectOrder.Percentage:
                            query = query.OrderBy(q => q.Percentage);
                            break;
                        case ProjectOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProjectOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProjectOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProjectOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ProjectOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case ProjectOrder.StartDate:
                            query = query.OrderByDescending(q => q.StartDate);
                            break;
                        case ProjectOrder.FinishDate:
                            query = query.OrderByDescending(q => q.FinishDate);
                            break;
                        case ProjectOrder.Percentage:
                            query = query.OrderByDescending(q => q.Percentage);
                            break;
                        case ProjectOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }
        private async Task<List<Project>> DynamicSelect(IQueryable<ProjectDAO> query, ProjectFilter filter)
        {
            List<Project> Projects = await query.Select(q => new Project()
            {
                Id = filter.Selects.Contains(ProjectSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProjectSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ProjectSelect.Name) ? q.Code : default(string),
                Description = filter.Selects.Contains(ProjectSelect.Description) ? q.Code : default(string),
                StartDate = filter.Selects.Contains(ProjectSelect.StartDate) ? q.StartDate : default(DateTime),
                FinishDate = filter.Selects.Contains(ProjectSelect.FinishDate) ? q.FinishDate : default(DateTime),
                Percentage = filter.Selects.Contains(ProjectSelect.Percentage) ? q.Percentage : default(int),
                Status = filter.Selects.Contains(ProjectSelect.Code) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
            }).ToListAsync();
            return Projects;
        }
        public async Task<int> Count(ProjectFilter filter)
        {
            IQueryable<ProjectDAO> Projects = DataContext.Projects.AsNoTracking();
            Projects = DynamicFilter(Projects, filter);
            return await Projects.CountAsync();
        }
        public async Task<List<Project>> List(ProjectFilter filter)
        {
            if (filter == null) return new List<Project>();
            IQueryable<ProjectDAO> ProjectDAOs = DataContext.Projects.AsNoTracking();
            ProjectDAOs = DynamicFilter(ProjectDAOs, filter);
            ProjectDAOs = DynamicOrder(ProjectDAOs, filter);
            List<Project> Projects = await DynamicSelect(ProjectDAOs, filter);
            return Projects;
        }
        public async Task<List<Project>> List(List<long> Ids)
        {
            List<Project> Projects = await DataContext.Projects.AsNoTracking()
            .Where(x => Ids.Contains(x.Id)).Select(x => new Project()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                StartDate = x.StartDate,
                FinishDate = x.FinishDate,
                Percentage = x.Percentage,
                StatusId = x.StatusId,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListAsync();
            return Projects;
        }
        public async Task<Project> Get(long Id)
        {
            Project Project = await DataContext.Projects.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new Project()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                StartDate = x.StartDate,
                FinishDate = x.FinishDate,
                Percentage = x.Percentage,
                StatusId = x.StatusId,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultAsync();
            if (Project == null)
                return null;
            return Project;
        }
        public async Task<bool> Create(Project Project)
        {
            ProjectDAO ProjectDAO = new ProjectDAO();
            ProjectDAO.Id = Project.Id;
            ProjectDAO.Code = Project.Code;
            ProjectDAO.Name = Project.Name;
            ProjectDAO.Description = Project.Description;
            ProjectDAO.StartDate = Project.StartDate;
            ProjectDAO.FinishDate = Project.FinishDate;
            ProjectDAO.Percentage = Project.Percentage;
            ProjectDAO.StatusId = Project.StatusId;
            ProjectDAO.Used = false;
            ProjectDAO.CreatedAt = StaticParams.DateTimeNow;
            ProjectDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.Projects.Add(ProjectDAO);
            await DataContext.SaveChangesAsync();
            Project.Id = ProjectDAO.Id;
            return true;
        }
        public async Task<bool> Update(Project Project)
        {
            ProjectDAO ProjectDAO = DataContext.Projects.Where(x => x.Id == Project.Id).FirstOrDefault();
            if (ProjectDAO == null)
                return false;
            ProjectDAO.Id = Project.Id;
            ProjectDAO.Code = Project.Code;
            ProjectDAO.Name = Project.Name;
            ProjectDAO.Description = Project.Description;
            ProjectDAO.StartDate = Project.StartDate;
            ProjectDAO.FinishDate = Project.FinishDate;
            ProjectDAO.Percentage = Project.Percentage;
            ProjectDAO.StatusId = Project.StatusId;
            ProjectDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(Project Project)
        {
            await DataContext.Projects.Where(x => x.Id == Project.Id).UpdateFromQueryAsync(x => new ProjectDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }
        public async Task<bool> BulkMerge(List<Project> Projects)
        {
            List<ProjectDAO> ProjectDAOs = new List<ProjectDAO>();
            foreach (Project Project in Projects)
            {
                ProjectDAO ProjectDAO = new ProjectDAO();
                ProjectDAO.Id = Project.Id;
                ProjectDAO.Code = Project.Code;
                ProjectDAO.Name = Project.Name;
                ProjectDAO.Description = Project.Description;
                ProjectDAO.StartDate = Project.StartDate;
                ProjectDAO.FinishDate = Project.FinishDate;
                ProjectDAO.Percentage = Project.Percentage;
                ProjectDAO.StatusId = Project.StatusId;
                ProjectDAO.CreatedAt = StaticParams.DateTimeNow;
                ProjectDAO.UpdatedAt = StaticParams.DateTimeNow;
                ProjectDAOs.Add(ProjectDAO);
            }
            await DataContext.BulkMergeAsync(ProjectDAOs);
            return true;
        }
        public async Task<bool> BulkUpdate(List<Project> Projects)
        {
            List<ProjectDAO> ProjectDAOs = new List<ProjectDAO>();
            foreach (Project Project in Projects)
            {
                ProjectDAO ProjectDAO = new ProjectDAO();
                ProjectDAO.Id = Project.Id;
                ProjectDAO.Code = Project.Code;
                ProjectDAO.Name = Project.Name;
                ProjectDAO.Description = Project.Description;
                ProjectDAO.StartDate = Project.StartDate;
                ProjectDAO.FinishDate = Project.FinishDate;
                ProjectDAO.Percentage = Project.Percentage;
                ProjectDAO.StatusId = Project.StatusId;
                ProjectDAO.CreatedAt = StaticParams.DateTimeNow;
                ProjectDAO.UpdatedAt = StaticParams.DateTimeNow;
                ProjectDAOs.Add(ProjectDAO);
            }
            await DataContext.BulkUpdateAsync(ProjectDAOs);
            return true;
        }
        public async Task<bool> BulkDelete(List<Project> Projects)
        {
            List<long> Ids = Projects.Select(x => x.Id).ToList();
            await DataContext.Projects
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ProjectDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Projects.Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ProjectDAO { Used = true });
            return true;
        }
    }
}
