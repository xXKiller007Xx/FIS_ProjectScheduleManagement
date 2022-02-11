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
    public interface IEmployeeRepository
    {
        Task<int> Count(EmployeeFilter EmployeeFilter);
        Task<List<Employee>> List(EmployeeFilter EmployeeFilter);
        Task<List<Employee>> List(List<long> Ids);
        Task<Employee> Get(long Id);
        Task<bool> Create(Employee Employee);
        Task<bool> Update(Employee Employee);
        Task<bool> Delete(Employee Employee);
        Task<bool> BulkMerge(List<Employee> Employees);
        Task<bool> BulkUpdate(List<Employee> Employees);
        Task<bool> BulkDelete(List<Employee> Employees);
        Task<bool> Used(List<long> Ids);
    }
    public class EmployeeRepository:IEmployeeRepository
    {
        private DataContext DataContext;
        public EmployeeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }
        private IQueryable<EmployeeDAO> DynamicFilter(IQueryable<EmployeeDAO> query, EmployeeFilter filter)
        {
            if (filter == null)
                return query.Where(query => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.GenderId, filter.GenderId);
            query = query.Where(q => q.JobId, filter.JobId);
            query = query.Where(q => q.DateOfBirth, filter.DateOfBirth);
            query = query.Where(q => q.Address, filter.Address);
            query = query.Where(q => q.Phone, filter.Phone);
            query = query.Where(q => q.Email, filter.Email);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = OrFilter(query, filter);
            return query;
        }
        private IQueryable<EmployeeDAO> OrFilter(IQueryable<EmployeeDAO> query, EmployeeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<EmployeeDAO> initQuery = query.Where(q => false);
            foreach (EmployeeFilter EmployeeFilter in filter.OrFilter)
            {
                IQueryable<EmployeeDAO> queryable = query;
                query = query.Where(q => q.Id, filter.Id);
                query = query.Where(q => q.Code, filter.Code);
                query = query.Where(q => q.Name, filter.Name);
                query = query.Where(q => q.GenderId, filter.GenderId);
                query = query.Where(q => q.JobId, filter.JobId);
                query = query.Where(q => q.DateOfBirth, filter.DateOfBirth);
                query = query.Where(q => q.Address, filter.Address);
                query = query.Where(q => q.Phone, filter.Phone);
                query = query.Where(q => q.Email, filter.Email);
                query = query.Where(q => q.StatusId, filter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }
        private IQueryable<EmployeeDAO> DynamicOrder(IQueryable<EmployeeDAO> query, EmployeeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case EmployeeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case EmployeeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case EmployeeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case EmployeeOrder.Gender:
                            query = query.OrderBy(q => q.GenderId);
                            break;
                        case EmployeeOrder.DateOfBirth:
                            query = query.OrderBy(q => q.DateOfBirth);
                            break;
                        case EmployeeOrder.Job:
                            query = query.OrderBy(q => q.JobId);
                            break;
                        case EmployeeOrder.Address:
                            query = query.OrderBy(q => q.Address);
                            break;
                        case EmployeeOrder.Phone:
                            query = query.OrderBy(q => q.Phone);
                            break;
                        case EmployeeOrder.Email:
                            query = query.OrderBy(q => q.Email);
                            break;
                        case EmployeeOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case EmployeeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case EmployeeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case EmployeeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case EmployeeOrder.Gender:
                            query = query.OrderByDescending(q => q.GenderId);
                            break;
                        case EmployeeOrder.DateOfBirth:
                            query = query.OrderByDescending(q => q.DateOfBirth);
                            break;
                        case EmployeeOrder.Job:
                            query = query.OrderByDescending(q => q.JobId);
                            break;
                        case EmployeeOrder.Address:
                            query = query.OrderByDescending(q => q.Address);
                            break;
                        case EmployeeOrder.Phone:
                            query = query.OrderByDescending(q => q.Phone);
                            break;
                        case EmployeeOrder.Email:
                            query = query.OrderByDescending(q => q.Email);
                            break;
                        case EmployeeOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }
        private async Task<List<Employee>> DynamicSelect(IQueryable<EmployeeDAO> query, EmployeeFilter filter)
        {
            List<Employee> Employees = await query.Select(q => new Employee()
            {
                Id = filter.Selects.Contains(EmployeeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(EmployeeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(EmployeeSelect.Name) ? q.Code : default(string),
                Gender = filter.Selects.Contains(EmployeeSelect.Gender) && q.Gender != null ? new Gender()
                {
                    Id = q.Gender.Id,
                    Code = q.Gender.Code,
                    Name = q.Gender.Name,
                } : null,
                Job = filter.Selects.Contains(EmployeeSelect.Job) && q.Job != null ? new Job()
                {
                    Id = q.Job.Id,
                    Code = q.Job.Code,
                    Name = q.Job.Name,
                } : null,
                DateOfBirth = filter.Selects.Contains(EmployeeSelect.DateOfBirth) ? q.DateOfBirth : default(DateTime),
                Address = filter.Selects.Contains(EmployeeSelect.Code) ? q.Code : default(string),
                Phone = filter.Selects.Contains(EmployeeSelect.Code) ? q.Code : default(string),
                Email = filter.Selects.Contains(EmployeeSelect.Code) ? q.Code : default(string),
                Status = filter.Selects.Contains(EmployeeSelect.Code) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
            }).ToListAsync();
            return Employees;
        }

        public async Task<int> Count(EmployeeFilter filter)
        {
            IQueryable<EmployeeDAO> Employees = DataContext.Employees.AsNoTracking();
            Employees = DynamicFilter(Employees, filter);
            return await Employees.CountAsync();
        }

        public async Task<List<Employee>> List(EmployeeFilter filter)
        {
            if (filter == null) return new List<Employee>();
            IQueryable<EmployeeDAO> EmployeeDAOs = DataContext.Employees.AsNoTracking();
            EmployeeDAOs = DynamicFilter(EmployeeDAOs, filter);
            EmployeeDAOs = DynamicOrder(EmployeeDAOs, filter);
            List<Employee> Employees = await DynamicSelect(EmployeeDAOs, filter);
            return Employees;
        }

        public async Task<List<Employee>> List(List<long> Ids)
        {
            List<Employee> Employees = await DataContext.Employees.AsNoTracking()
            .Where(x => Ids.Contains(x.Id)).Select(x => new Employee()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                GenderId = x.GenderId,
                JobId = x.JobId,
                DateOfBirth = x.DateOfBirth,
                Address = x.Address,
                Phone = x.Phone,
                Email = x.Email,
                StatusId = x.StatusId,
                Gender = x.Gender == null ? null : new Gender()
                {
                    Id = x.Gender.Id,
                    Code = x.Gender.Code,
                    Name = x.Gender.Name,
                },
                Job = x.Job == null ? null : new Job()
                {
                    Id = x.Job.Id,
                    Code = x.Job.Code,
                    Name = x.Job.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListAsync();
            return Employees;
        }

        public async Task<Employee> Get(long Id)
        {
            Employee Employee = await DataContext.Employees.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new Employee()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                GenderId = x.GenderId,
                JobId = x.JobId,
                DateOfBirth = x.DateOfBirth,
                Address = x.Address,
                Phone = x.Phone,
                Email = x.Email,
                StatusId = x.StatusId,
                Gender = x.Gender == null ? null : new Gender()
                {
                    Id = x.Gender.Id,
                    Code = x.Gender.Code,
                    Name = x.Gender.Name,
                },
                Job = x.Job == null ? null : new Job()
                {
                    Id = x.Job.Id,
                    Code = x.Job.Code,
                    Name = x.Job.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultAsync();
            if (Employee == null)
                return null;
            return Employee;
        }

        public async Task<bool> Create(Employee Employee)
        {
            EmployeeDAO EmployeeDAO = new EmployeeDAO();
            EmployeeDAO.Id = Employee.Id;
            EmployeeDAO.Code = Employee.Code;
            EmployeeDAO.Name = Employee.Name;
            EmployeeDAO.GenderId = Employee.GenderId;
            EmployeeDAO.JobId = Employee.JobId;
            EmployeeDAO.DateOfBirth = Employee.DateOfBirth;
            EmployeeDAO.Address = Employee.Address;
            EmployeeDAO.Phone = Employee.Phone;
            EmployeeDAO.Email = Employee.Email;
            EmployeeDAO.StatusId = Employee.StatusId;
            EmployeeDAO.Used = false;
            EmployeeDAO.CreatedAt = DateTime.Now;
            EmployeeDAO.UpdatedAt = DateTime.Now;
            DataContext.Employees.Add(EmployeeDAO);
            await DataContext.SaveChangesAsync();
            Employee.Id = EmployeeDAO.Id;
            return true;
        }

        public async Task<bool> Update(Employee Employee)
        {
            EmployeeDAO EmployeeDAO = DataContext.Employees.Where(x => x.Id == Employee.Id).FirstOrDefault();
            if (EmployeeDAO == null)
                return false;
            EmployeeDAO.Id = Employee.Id;
            EmployeeDAO.Code = Employee.Code;
            EmployeeDAO.Name = Employee.Name;
            EmployeeDAO.GenderId = Employee.GenderId;
            EmployeeDAO.JobId = Employee.JobId;
            EmployeeDAO.DateOfBirth = Employee.DateOfBirth;
            EmployeeDAO.Address = Employee.Address;
            EmployeeDAO.Phone = Employee.Phone;
            EmployeeDAO.Email = Employee.Email;
            EmployeeDAO.StatusId = Employee.StatusId;
            EmployeeDAO.UpdatedAt = DateTime.Now;
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Employee Employee)
        {
            List<TaskEmployeeMappingDAO> TaskEmployeeMappingDAOs = DataContext.TaskEmployeeMappings.Where(t => t.EmployeeId == Employee.Id).ToList();
            foreach (var item in TaskEmployeeMappingDAOs)
            {
                DataContext.TaskEmployeeMappings.Remove(item);
                await DataContext.SaveChangesAsync();
            }
            await DataContext.Employees.Where(x => x.Id == Employee.Id).UpdateFromQueryAsync(x => new EmployeeDAO { DeletedAt = DateTime.Now });
            return true;
        }

        public async Task<bool> BulkMerge(List<Employee> Employees)
        {
            List<EmployeeDAO> EmployeeDAOs = new List<EmployeeDAO>();
            foreach (Employee Employee in Employees)
            {
                EmployeeDAO EmployeeDAO = new EmployeeDAO();
                EmployeeDAO.Id = Employee.Id;
                EmployeeDAO.Code = Employee.Code;
                EmployeeDAO.Name = Employee.Name;
                EmployeeDAO.GenderId = Employee.GenderId;
                EmployeeDAO.JobId = Employee.JobId;
                EmployeeDAO.DateOfBirth = Employee.DateOfBirth;
                EmployeeDAO.Address = Employee.Address;
                EmployeeDAO.Phone = Employee.Phone;
                EmployeeDAO.Email = Employee.Email;
                EmployeeDAO.StatusId = Employee.StatusId;
                EmployeeDAO.CreatedAt = StaticParams.DateTimeNow;
                EmployeeDAO.UpdatedAt = StaticParams.DateTimeNow;
                EmployeeDAOs.Add(EmployeeDAO);
            }
            await DataContext.BulkMergeAsync(EmployeeDAOs);
            return true;
        }

        public async Task<bool> BulkUpdate(List<Employee> Employees)
        {
            List<EmployeeDAO> EmployeeDAOs = new List<EmployeeDAO>();
            foreach (Employee Employee in Employees)
            {
                EmployeeDAO EmployeeDAO = new EmployeeDAO();
                EmployeeDAO.Id = Employee.Id;
                EmployeeDAO.Code = Employee.Code;
                EmployeeDAO.Name = Employee.Name;
                EmployeeDAO.GenderId = Employee.GenderId;
                EmployeeDAO.JobId = Employee.JobId;
                EmployeeDAO.DateOfBirth = Employee.DateOfBirth;
                EmployeeDAO.Address = Employee.Address;
                EmployeeDAO.Phone = Employee.Phone;
                EmployeeDAO.Email = Employee.Email;
                EmployeeDAO.StatusId = Employee.StatusId;
                EmployeeDAO.CreatedAt = StaticParams.DateTimeNow;
                EmployeeDAO.UpdatedAt = StaticParams.DateTimeNow;
                EmployeeDAOs.Add(EmployeeDAO);
            }
            await DataContext.BulkUpdateAsync(EmployeeDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<Employee> Employees)
        {
            List<long> Ids = Employees.Select(x => x.Id).ToList();
            await DataContext.Employees
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new EmployeeDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Employees.Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new EmployeeDAO { Used = true });
            return true;
        }
    }
}
