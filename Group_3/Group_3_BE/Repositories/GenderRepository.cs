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
    public interface IGenderRepository
    {
        Task<int> Count(GenderFilter GenderFilter);
        Task<List<Gender>> List(GenderFilter GenderFilter);
        Task<Gender> Get(long Id);
    }
    public class GenderRepository : IGenderRepository
    {
        private DataContext DataContext;
        public GenderRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }
        private IQueryable<GenderDAO> DynamicFilter(IQueryable<GenderDAO> query, GenderFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = OrFilter(query, filter);
            return query;
        }
        private IQueryable<GenderDAO> OrFilter(IQueryable<GenderDAO> query, GenderFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<GenderDAO> initQuery = query.Where(q => false);
            foreach (GenderFilter GenderFilter in filter.OrFilter)
            {
                IQueryable<GenderDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, GenderFilter.Id);
                queryable = queryable.Where(q => q.Code, GenderFilter.Code);
                queryable = queryable.Where(q => q.Name, GenderFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }
        private IQueryable<GenderDAO> DynamicOrder(IQueryable<GenderDAO> query, GenderFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case GenderOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case GenderOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case GenderOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case GenderOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case GenderOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case GenderOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }
        private async Task<List<Gender>> DynamicSelect(IQueryable<GenderDAO> query, GenderFilter filter)
        {
            List<Gender> Genders = await query.Select(q => new Gender()
            {
                Id = filter.Selects.Contains(GenderSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(GenderSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(GenderSelect.Name) ? q.Name : default(string),
            }).ToListAsync();
            return Genders;
        }
        public async Task<int> Count(GenderFilter filter)
        {
            IQueryable<GenderDAO> Gender = DataContext.Genders;
            Gender = DynamicFilter(Gender, filter);
            return await Gender.CountAsync();
        }

        public async Task<Gender> Get(long Id)
        {
            Gender Gender = await DataContext.Genders.Where(x => x.Id == Id).Select(x => new Gender()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultAsync();
            if (Gender == null)
                return null;
            return Gender;
        }

        public async Task<List<Gender>> List(GenderFilter filter)
        {
            if (filter == null) return new List<Gender>();
            IQueryable<GenderDAO> GenderDAOs = DataContext.Genders;
            GenderDAOs = DynamicFilter(GenderDAOs, filter);
            GenderDAOs = DynamicOrder(GenderDAOs, filter);
            List<Gender> Genders = await DynamicSelect(GenderDAOs, filter);
            return Genders;
        }
    }
}
