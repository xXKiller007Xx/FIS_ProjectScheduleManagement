using Group_3_BE.Entities;
using Group_3_BE.Helpers;
using Group_3_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Repositories
{
    public interface ITaskTypeRepository
    {
        Task<int> Count(TaskTypeFilter TaskTypeFilter);
        Task<List<TaskType>> List(TaskTypeFilter TaskTypeFilter);
        Task<List<TaskType>> List(List<long> Ids);
        Task<TaskType> Get(long Id);
        Task<bool> Create(TaskType TaskType);
        Task<bool> Update(TaskType TaskType);
        Task<bool> Delete(TaskType TaskType);
        Task<bool> BulkMerge(List<TaskType> TaskTypes);
        Task<bool> BulkUpdate(List<TaskType> TaskTypes);
        Task<bool> BulkDelete(List<TaskType> TaskTypes);
        //Task<bool> Used(List<long> Ids);
    }
    public class TaskTypeRepository : ITaskTypeRepository
    {
        private DataContext DataContext;
        public TaskTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<TaskTypeDAO> DynamicFilter(IQueryable<TaskTypeDAO> query, TaskTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);

            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<TaskTypeDAO> OrFilter(IQueryable<TaskTypeDAO> query, TaskTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<TaskTypeDAO> initQuery = query.Where(q => false);
            foreach (TaskTypeFilter TaskTypeFilter in filter.OrFilter)
            {
                IQueryable<TaskTypeDAO> queryable = query;
                queryable = queryable.Where(q => !q.DeletedAt.HasValue);
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.Name, filter.Name);
                queryable = queryable.Where(q => q.Description, filter.Description);
                queryable = queryable.Where(q => q.StatusId, filter.StatusId);
                queryable = queryable.Where(q => q.UpdatedAt, filter.UpdatedAt);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<TaskTypeDAO> DynamicOrder(IQueryable<TaskTypeDAO> query, TaskTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case TaskTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case TaskTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case TaskTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case TaskTypeOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case TaskTypeOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case TaskTypeOrder.UpdatedAt:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                        default:
                            query = query.OrderBy(q => q.CreatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case TaskTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case TaskTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case TaskTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case TaskTypeOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case TaskTypeOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case TaskTypeOrder.UpdatedAt:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                        default:
                            query = query.OrderByDescending(q => q.CreatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<TaskType>> DynamicSelect(IQueryable<TaskTypeDAO> query, TaskTypeFilter filter)
        {
            List<TaskType> TaskTypes = await query.Select(q => new TaskType()
            {
                Id = filter.Selects.Contains(TaskTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(TaskTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(TaskTypeSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(TaskTypeSelect.Description) ? q.Description : default(string),
                StatusId = filter.Selects.Contains(TaskTypeSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(TaskTypeSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                DeletedAt = q.DeletedAt,
            }).ToListAsync();
            // Có vẻ thiếu lấy List Tasks tương ứng vs từng phần tử của List TaskTypes
            return TaskTypes;
        }

        public async Task<int> Count(TaskTypeFilter filter)
        {
            IQueryable<TaskTypeDAO> TaskTypes = DataContext.TaskTypes;
            TaskTypes = DynamicFilter(TaskTypes, filter);
            return await TaskTypes.CountAsync();
        }

        public async Task<List<TaskType>> List(TaskTypeFilter filter)
        {
            if (filter == null) return new List<TaskType>();
            IQueryable<TaskTypeDAO> TaskTypeDAOs = DataContext.TaskTypes.AsNoTracking();
            TaskTypeDAOs = DynamicFilter(TaskTypeDAOs, filter);
            TaskTypeDAOs = DynamicOrder(TaskTypeDAOs, filter);
            List<TaskType> TaskTypes = await DynamicSelect(TaskTypeDAOs, filter);
            return TaskTypes;
        }

        public async Task<List<TaskType>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<TaskType> TaskTypes = await DataContext.TaskTypes.AsNoTracking()
                .Where(x => Ids.Contains(x.Id)).Select(x => new TaskType()
                {
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,

                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,

                    StatusId = x.StatusId,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).ToListAsync();
                List<Entities.Task> Tasks = await DataContext.Tasks.AsNoTracking()
                    .Where(x => x.TaskTypeId, IdFilter)
                    .Select(x => new Entities.Task()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Description = x.Description,
                        StartDate = x.StartDate,
                        FinishDate = x.FinishDate,
                        Percentage = x.Percentage,
                        ProjectId = x.ProjectId,
                        TaskTypeId = x.TaskTypeId,
                        StatusId = x.StatusId,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        DeletedAt = x.DeletedAt,
                        Used = x.Used,
                        Status = new Status
                        {
                            Id = x.Status.Id,
                            Code = x.Status.Code,
                            Name = x.Status.Name,
                        },
                    }).ToListAsync();
                foreach(TaskType TaskType in TaskTypes)
                {
                    TaskType.Tasks = Tasks
                        .Where(x => x.TaskTypeId == TaskType.Id)
                        .ToList();
                }
            return TaskTypes;
        }

        public async Task<TaskType> Get(long Id)
        {
            TaskType TaskType = await DataContext.TaskTypes.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new TaskType()
                {
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,

                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,

                    StatusId = x.StatusId,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).FirstOrDefaultAsync();

            TaskType.Tasks = await DataContext.Tasks.AsNoTracking()
                    .Where(x => x.TaskTypeId == TaskType.Id)
                    .Where(x => x.DeletedAt == null)
                    .Select(x => new Entities.Task()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Description = x.Description,
                        StartDate = x.StartDate,
                        FinishDate = x.FinishDate,
                        Percentage = x.Percentage,
                        ProjectId = x.ProjectId,
                        TaskTypeId = x.TaskTypeId,
                        StatusId = x.StatusId,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        DeletedAt = x.DeletedAt,
                        Used = x.Used,
                        Status = new Status
                        {
                            Id = x.Status.Id,
                            Code = x.Status.Code,
                            Name = x.Status.Name,
                        },
                    }).ToListAsync();

            if (TaskType == null) return null;
            return TaskType;
        }

        public async Task<bool> Create(TaskType TaskType)
        {
            TaskTypeDAO TaskTypeDAO = new TaskTypeDAO();
            TaskTypeDAO.Id = TaskType.Id;
            TaskTypeDAO.Code = TaskType.Code;
            TaskTypeDAO.Name = TaskType.Name;
            TaskTypeDAO.Description = TaskType.Description;
            TaskTypeDAO.CreatedAt = StaticParams.DateTimeNow;
            TaskTypeDAO.UpdatedAt = StaticParams.DateTimeNow;
            TaskTypeDAO.StatusId = TaskType.StatusId;

            DataContext.TaskTypes.Add(TaskTypeDAO);
            await DataContext.SaveChangesAsync();
            TaskType.Id = TaskTypeDAO.Id;
            return true;
        }

        public async Task<bool> Update(TaskType TaskType)
        {
            TaskTypeDAO TaskTypeDAO = DataContext.TaskTypes.Where(x => x.Id == TaskType.Id).FirstOrDefault();
            if (TaskTypeDAO == null)
                return false;

            TaskTypeDAO.Id = TaskType.Id;
            TaskTypeDAO.Code = TaskType.Code;
            TaskTypeDAO.Name = TaskType.Name;
            TaskTypeDAO.Description = TaskType.Description;
            TaskTypeDAO.UpdatedAt = StaticParams.DateTimeNow;
            TaskTypeDAO.StatusId = TaskType.StatusId;
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(TaskType TaskType)
        {
            await DataContext.Tasks
                .Where(x => x.TaskTypeId.HasValue && x.TaskTypeId.Value == TaskType.Id)
                .UpdateFromQueryAsync(x => new TaskDAO { TaskTypeId = null });
            await DataContext.TaskTypes.Where(x => x.Id == TaskType.Id)
                .UpdateFromQueryAsync(x => new TaskTypeDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<TaskType> TaskTypes)
        {
            List<TaskTypeDAO> TaskTypeDAOs = new List<TaskTypeDAO>();
            foreach (TaskType TaskType in TaskTypes)
            {
                TaskTypeDAO TaskTypeDAO = new TaskTypeDAO();
                TaskTypeDAO.Id = TaskType.Id;
                TaskTypeDAO.Code = TaskType.Code;
                TaskTypeDAO.Name = TaskType.Name;
                TaskTypeDAO.Description = TaskType.Description;
                TaskTypeDAO.CreatedAt = StaticParams.DateTimeNow;
                TaskTypeDAO.UpdatedAt = StaticParams.DateTimeNow;
                TaskTypeDAO.StatusId = TaskType.StatusId;

                DataContext.TaskTypes.Add(TaskTypeDAO);
            }
            await DataContext.BulkMergeAsync(TaskTypeDAOs);
            return true;
        }
        public async Task<bool> BulkUpdate(List<TaskType> TaskTypes)
        {
            List<TaskTypeDAO> TaskTypeDAOs = new List<TaskTypeDAO>();
            foreach (TaskType TaskType in TaskTypes)
            {
                TaskTypeDAO TaskTypeDAO = new TaskTypeDAO();
                TaskTypeDAO.Id = TaskType.Id;
                TaskTypeDAO.Code = TaskType.Code;
                TaskTypeDAO.Name = TaskType.Name;
                TaskTypeDAO.Description = TaskType.Description;
                TaskTypeDAO.CreatedAt = StaticParams.DateTimeNow;
                TaskTypeDAO.UpdatedAt = StaticParams.DateTimeNow;
                TaskTypeDAO.StatusId = TaskType.StatusId;

                DataContext.TaskTypes.Add(TaskTypeDAO);
            }
            await DataContext.BulkUpdateAsync(TaskTypeDAOs);
            return true;
        }
        public async Task<bool> BulkDelete(List<TaskType> TaskTypes)
        {
            List<long> Ids = TaskTypes.Select(x => x.Id).ToList();
            await DataContext.Tasks
                .Where(x => x.TaskTypeId.HasValue && Ids.Contains(x.TaskTypeId.Value))
                .UpdateFromQueryAsync(x => new TaskDAO { TaskTypeId = null });
            await DataContext.TaskTypes.Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new TaskTypeDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

    }
}
