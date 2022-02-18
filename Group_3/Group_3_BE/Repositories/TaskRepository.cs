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
    public interface ITaskRepository
    {
        Task<int> Count(TaskFilter TaskFilter);
        Task<List<Entities.Task>> List(TaskFilter TaskFilter);
        Task<List<Entities.Task>> List(List<long> Ids);
        Task<Entities.Task> Get(long Id);
        Task<bool> Create(Entities.Task Task);
        Task<bool> Update(Entities.Task Task);
        Task<bool> Delete(Entities.Task Task);
        Task<bool> BulkMerge(List<Entities.Task> Tasks);
        Task<bool> BulkUpdate(List<Entities.Task> Tasks);
        Task<bool> BulkDelete(List<Entities.Task> Tasks);
        Task<bool> Used(List<long> Ids);
    }
    public class TaskRepository : ITaskRepository
    {
        private DataContext DataContext;
        public TaskRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<TaskDAO> DynamicFilter(IQueryable<TaskDAO> query, TaskFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);

            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.StartDate, filter.StartDate);
            query = query.Where(q => q.FinishDate, filter.FinishDate);
            query = query.Where(q => q.Percentage, filter.Percentage);
            query = query.Where(q => q.ProjectId, filter.ProjectId);
            query = query.Where(q => q.TaskTypeId, filter.TaskTypeId);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                List<string> Tokens = filter.Search.Split(" ").Select(x => x.ToLower()).ToList();
                var queryForCode = query;
                var queryForName = query;
                foreach (string Token in Tokens)
                {
                    if (string.IsNullOrWhiteSpace(Token))
                        continue;
                    queryForCode = queryForCode.Where(x => x.Code.ToLower().Contains(Token));
                    queryForName = queryForName.Where(x => x.Name.ToLower().Contains(Token));
                }
                query = queryForCode.Union(queryForName);
                query = query.Distinct();
            }

            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<TaskDAO> OrFilter(IQueryable<TaskDAO> query, TaskFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<TaskDAO> initQuery = query.Where(q => false);
            foreach (TaskFilter TaskFilter in filter.OrFilter)
            {
                IQueryable<TaskDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.Name, filter.Name);
                queryable = queryable.Where(q => q.StatusId, filter.StatusId);
                queryable = queryable.Where(q => q.Description, filter.Description);
                queryable = queryable.Where(q => q.StartDate, filter.StartDate);
                queryable = queryable.Where(q => q.FinishDate, filter.FinishDate);
                queryable = queryable.Where(q => q.Percentage, filter.Percentage);
                queryable = queryable.Where(q => q.ProjectId, filter.ProjectId);
                queryable = queryable.Where(q => q.TaskTypeId, filter.TaskTypeId);
                queryable = queryable.Where(q => q.CreatedAt, filter.CreatedAt);
                queryable = queryable.Where(q => q.UpdatedAt, filter.UpdatedAt);
                initQuery = initQuery.Union(queryable);

            }
            return initQuery;
        }

        private IQueryable<TaskDAO> DynamicOrder(IQueryable<TaskDAO> query, TaskFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case TaskOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case TaskOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case TaskOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case TaskOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case TaskOrder.StartDate:
                            query = query.OrderBy(q => q.StartDate);
                            break;
                        case TaskOrder.FinishDate:
                            query = query.OrderBy(q => q.FinishDate);
                            break;
                        case TaskOrder.Percentage:
                            query = query.OrderBy(q => q.Percentage);
                            break;
                        case TaskOrder.Project:
                            query = query.OrderBy(q => q.Project);
                            break;
                        case TaskOrder.UpdatedAt:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                        case TaskOrder.TaskType:
                            query = query.OrderBy(q => q.TaskType);
                            break;
                        case TaskOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case TaskOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case TaskOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case TaskOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case TaskOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case TaskOrder.StartDate:
                            query = query.OrderByDescending(q => q.StartDate);
                            break;
                        case TaskOrder.FinishDate:
                            query = query.OrderByDescending(q => q.FinishDate);
                            break;
                        case TaskOrder.Percentage:
                            query = query.OrderByDescending(q => q.Percentage);
                            break;
                        case TaskOrder.Project:
                            query = query.OrderByDescending(q => q.Project);
                            break;
                        case TaskOrder.UpdatedAt:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                        case TaskOrder.TaskType:
                            query = query.OrderByDescending(q => q.TaskType);
                            break;
                        case TaskOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Entities.Task>> DynamicSelect(IQueryable<TaskDAO> query, TaskFilter filter)
        {
            List<Entities.Task> Tasks = await query.Select(q => new Entities.Task()
            {
                Id = filter.Selects.Contains(TaskSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(TaskSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(TaskSelect.Name) ? q.Code : default(string),
                Description = filter.Selects.Contains(TaskSelect.Description) ? q.Code : default(string),
                StartDate = filter.Selects.Contains(TaskSelect.StartDate) ? q.StartDate : default(DateTime),
                FinishDate = filter.Selects.Contains(TaskSelect.FinishDate) ? q.FinishDate : default(DateTime),
                Percentage = filter.Selects.Contains(TaskSelect.Percentage) ? q.Percentage : default(int),

                TaskTypeId = filter.Selects.Contains(TaskSelect.TaskType) ? q.TaskTypeId : default(long),
                TaskType = filter.Selects.Contains(TaskSelect.TaskType) && q.Status != null ? new TaskType
                {
                    CreatedAt = q.TaskType.CreatedAt,
                    UpdatedAt = q.TaskType.UpdatedAt,
                    DeletedAt = q.TaskType.DeletedAt,

                    Id = q.TaskType.Id,
                    Code = q.TaskType.Code,
                    Name = q.TaskType.Name,
                    Description = q.TaskType.Description,

                    StatusId = q.TaskType.StatusId,
                } : null,

                StatusId = filter.Selects.Contains(TaskSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(TaskSelect.Code) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
            }).ToListAsync();
            return Tasks;
        }

        public async Task<int> Count(TaskFilter filter)
        {
            IQueryable<TaskDAO> Tasks = DataContext.Tasks;
            Tasks = DynamicFilter(Tasks, filter);
            return await Tasks.CountAsync();
        }

        public async Task<List<Entities.Task>> List(TaskFilter filter)
        {
            if (filter == null) return new List<Entities.Task>();
            IQueryable<TaskDAO> TaskDAOs = DataContext.Tasks.AsNoTracking();
            TaskDAOs = DynamicFilter(TaskDAOs, filter);
            TaskDAOs = DynamicOrder(TaskDAOs, filter);
            List<Entities.Task> Tasks = await DynamicSelect(TaskDAOs, filter);
            return Tasks;
        }

        public async Task<List<Entities.Task>> List(List<long> Ids)
        {
            List<Entities.Task> Tasks = await DataContext.Tasks.AsNoTracking()
                .Where(x => Ids.Contains(x.Id)).Select(x => new Entities.Task()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    FinishDate = x.FinishDate,
                    Percentage = x.Percentage,
                    TaskTypeId = x.TaskTypeId,
                    TaskType = x.TaskType == null ? null : new TaskType
                    {
                        CreatedAt = x.TaskType.CreatedAt,
                        UpdatedAt = x.TaskType.UpdatedAt,
                        DeletedAt = x.TaskType.DeletedAt,

                        Id = x.TaskType.Id,
                        Code = x.TaskType.Code,
                        Name = x.TaskType.Name,
                        Description = x.TaskType.Description,

                        StatusId = x.TaskType.StatusId,
                        Status = x.Status == null ? null : new Status
                        {
                            Id = x.TaskType.Status.Id,
                            Code = x.TaskType.Status.Code,
                            Name = x.TaskType.Status.Name,
                        },
                    },
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    StatusId = x.StatusId,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },

                }).ToListAsync();
            return Tasks;
        }

        public async Task<Entities.Task> Get(long Id)
        {
            Entities.Task Task = await DataContext.Tasks.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new Entities.Task()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    FinishDate = x.FinishDate,
                    Percentage = x.Percentage,
                    TaskTypeId = x.TaskTypeId,
                    TaskType = x.TaskType == null ? null : new TaskType
                    {
                        CreatedAt = x.TaskType.CreatedAt,
                        UpdatedAt = x.TaskType.UpdatedAt,
                        DeletedAt = x.TaskType.DeletedAt,

                        Id = x.TaskType.Id,
                        Code = x.TaskType.Code,
                        Name = x.TaskType.Name,
                        Description = x.TaskType.Description,

                        StatusId = x.TaskType.StatusId,
                        Status = x.Status == null ? null : new Status
                        {
                            Id = x.TaskType.Status.Id,
                            Code = x.TaskType.Status.Code,
                            Name = x.TaskType.Status.Name,
                        },
                    },
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    StatusId = x.StatusId,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).FirstOrDefaultAsync();
            if (Task == null)
                return null;
            return Task;
        }
        public async Task<bool> Create(Entities.Task Task)
        {
            TaskDAO TaskDAO = new TaskDAO();
            TaskDAO.Id = Task.Id;
            TaskDAO.Code = Task.Code;
            TaskDAO.Name = Task.Name;
            TaskDAO.Description = Task.Description;
            TaskDAO.StartDate = Task.StartDate;
            TaskDAO.FinishDate = Task.FinishDate;
            TaskDAO.Percentage = Task.Percentage;
            TaskDAO.ProjectId = Task.ProjectId;
            TaskDAO.TaskTypeId = Task.TaskTypeId;
            TaskDAO.Used = false;
            TaskDAO.CreatedAt = StaticParams.DateTimeNow;
            TaskDAO.UpdatedAt = StaticParams.DateTimeNow;
            TaskDAO.StatusId = Task.StatusId;

            DataContext.Tasks.Add(TaskDAO);
            await DataContext.SaveChangesAsync();
            await SaveReference(Task);
            Task.Id = TaskDAO.Id;
            return true;
        }
        public async Task<bool> Update(Entities.Task Task)
        {
            TaskDAO TaskDAO = DataContext.Tasks.Where(x => x.Id == Task.Id).FirstOrDefault();
            if (TaskDAO == null)
                return false;
            TaskDAO.Id = Task.Id;
            TaskDAO.Code = Task.Code;
            TaskDAO.Name = Task.Name;
            TaskDAO.Description = Task.Description;
            TaskDAO.StartDate = Task.StartDate;
            TaskDAO.FinishDate = Task.FinishDate;
            TaskDAO.Percentage = Task.Percentage;
            TaskDAO.ProjectId = Task.ProjectId;
            TaskDAO.TaskTypeId = Task.TaskTypeId;
            TaskDAO.Used = false;
            TaskDAO.UpdatedAt = StaticParams.DateTimeNow;
            TaskDAO.StatusId = Task.StatusId;

            await DataContext.SaveChangesAsync();
            await SaveReference(Task);
            return true;
        }
        public async Task<bool> Delete(Entities.Task Task)
        {
            await DataContext.Tasks.Where(x => x.Id == Task.Id)
                .UpdateFromQueryAsync(x => new TaskTypeDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }
        public async Task<bool> BulkMerge(List<Entities.Task> Tasks)
        {
            List<TaskDAO> TaskDAOs = new List<TaskDAO>();
            foreach (Entities.Task Task in Tasks)
            {
                TaskDAO TaskDAO = new TaskDAO();
                TaskDAO.Id = Task.Id;
                TaskDAO.Code = Task.Code;
                TaskDAO.Name = Task.Name;
                TaskDAO.Description = Task.Description;
                TaskDAO.StartDate = Task.StartDate;
                TaskDAO.FinishDate = Task.FinishDate;
                TaskDAO.Percentage = Task.Percentage;
                TaskDAO.ProjectId = Task.ProjectId;
                TaskDAO.TaskTypeId = Task.TaskTypeId;
                TaskDAO.Used = false;
                TaskDAO.CreatedAt = StaticParams.DateTimeNow;
                TaskDAO.UpdatedAt = StaticParams.DateTimeNow;
                TaskDAO.StatusId = Task.StatusId;

                TaskDAOs.Add(TaskDAO);
            }
            await DataContext.BulkMergeAsync(TaskDAOs);
            return true;
        }
        public async Task<bool> BulkUpdate(List<Entities.Task> Tasks)
        {
            List<TaskDAO> TaskDAOs = new List<TaskDAO>();
            foreach (Entities.Task Task in Tasks)
            {
                TaskDAO TaskDAO = new TaskDAO();
                TaskDAO.Id = Task.Id;
                TaskDAO.Code = Task.Code;
                TaskDAO.Name = Task.Name;
                TaskDAO.Description = Task.Description;
                TaskDAO.StartDate = Task.StartDate;
                TaskDAO.FinishDate = Task.FinishDate;
                TaskDAO.Percentage = Task.Percentage;
                TaskDAO.ProjectId = Task.ProjectId;
                TaskDAO.TaskTypeId = Task.TaskTypeId;
                TaskDAO.Used = false;
                TaskDAO.CreatedAt = StaticParams.DateTimeNow;
                TaskDAO.UpdatedAt = StaticParams.DateTimeNow;
                TaskDAO.StatusId = Task.StatusId;

                TaskDAOs.Add(TaskDAO);
            }
            await DataContext.BulkUpdateAsync(TaskDAOs);
            return true;
        }
        public async Task<bool> BulkDelete(List<Entities.Task> Tasks)
        {
            List<long> Ids = Tasks.Select(x => x.Id).ToList();
            await DataContext.Tasks
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new TaskDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async System.Threading.Tasks.Task SaveReference(Entities.Task Task)
        {
            await DataContext.TaskEmployeeMappings
                .Where(x => x.TaskId == Task.Id)
                .DeleteFromQueryAsync();
            List<TaskEmployeeMappingDAO> TaskEmployeeMappingDAOs = new List<TaskEmployeeMappingDAO>();
            if (Task.TaskEmployeeMappings != null)
            {
                foreach (TaskEmployeeMapping TaskEmployeeMapping in Task.TaskEmployeeMappings)
                {
                    TaskEmployeeMappingDAO TaskEmployeeMappingDAO = new TaskEmployeeMappingDAO()
                    {
                        TaskId = Task.Id,
                        EmployeeId = TaskEmployeeMapping.EmployeeId,
                    };
                    TaskEmployeeMappingDAOs.Add(TaskEmployeeMappingDAO);
                }
                await DataContext.TaskEmployeeMappings.BulkMergeAsync(TaskEmployeeMappingDAOs);
            }
        }

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Tasks.Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new TaskDAO { Used = true });
            return true;
        }
    }
}
