using Group_3_BE.Entities;
using Group_3_BE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Repositories
{
    public interface IJobRepository
    {
        Task<int> Count(JobFilter GenderFilter);
        Task<List<Job>> List(JobFilter GenderFilter);
        Task<Job> Get(long Id);
    }
    public class JobRepository : IJobRepository
    {
        private DataContext DataContext;
        public JobRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }
        private IQueryable<JobDAO> DynamicFilter(IQueryable<JobDAO> query, JobFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = OrFilter(query, filter);
            return query;
        }
        private IQueryable<JobDAO> OrFilter(IQueryable<JobDAO> query, JobFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<JobDAO> initQuery = query.Where(q => false);
            foreach (JobFilter GenderFilter in filter.OrFilter)
            {
                IQueryable<JobDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, GenderFilter.Id);
                queryable = queryable.Where(q => q.Code, GenderFilter.Code);
                queryable = queryable.Where(q => q.Name, GenderFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }
        private IQueryable<JobDAO> DynamicOrder(IQueryable<JobDAO> query, JobFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case JobOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case JobOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case JobOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case JobOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case JobOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case JobOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }
        private async Task<List<Job>> DynamicSelect(IQueryable<JobDAO> query, JobFilter filter)
        {
            List<Job> Jobs = await query.Select(q => new Job()
            {
                Id = filter.Selects.Contains(JobSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(JobSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(JobSelect.Name) ? q.Name : default(string),
            }).ToListAsync();
            return Jobs;
        }
        public async Task<int> Count(JobFilter filter)
        {
            IQueryable<JobDAO> Job = DataContext.Jobs;
            Job = DynamicFilter(Job, filter);
            return await Job.CountAsync();
        }

        public async Task<Job> Get(long Id)
        {
            Job Job = await DataContext.Genders.Where(x => x.Id == Id).Select(x => new Job()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultAsync();
            if (Job == null)
                return null;
            return Job;
        }

        public async Task<List<Job>> List(JobFilter filter)
        {
            if (filter == null) return new List<Job>();
            IQueryable<JobDAO> JobDAOs = DataContext.Jobs;
            JobDAOs = DynamicFilter(JobDAOs, filter);
            JobDAOs = DynamicOrder(JobDAOs, filter);
            List<Job> Jobs = await DynamicSelect(JobDAOs, filter);
            return Jobs;
        }
    }
}
