using Group_3_BE.Entities;
using Group_3_BE.Helpers;
using Group_3_BE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MEmployee
{
    public interface IEmployeeService : IServiceScoped
    {
        Task<int> Count(EmployeeFilter EmployeeFilter);
        Task<List<Employee>> List(EmployeeFilter EmployeeFilter);
        Task<Employee> Get(long Id);
        Task<Employee> Create(Employee Employee);
        Task<Employee> Update(Employee Employee);
        Task<Employee> Delete(Employee Employee);
        Task<List<Employee>> BulkMerge(List<Employee> Employees);
        EmployeeFilter ToFilter(EmployeeFilter EmployeeFilter);
        Task<List<Employee>> Import(List<Employee> Employees);
    }
    public class EmployeeService : IEmployeeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private IEmployeeValidator EmployeeValidator;
        public EmployeeService
        (
            IUOW UOW,
            ILogging Logging,
            IEmployeeValidator EmployeeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.EmployeeValidator = EmployeeValidator;
        }
        public async Task<List<Employee>> BulkMerge(List<Employee> Employees)
        {
            if (!await EmployeeValidator.BulkMerge(Employees))
                return Employees;
            try
            {
                await UOW.EmployeeRepository.BulkMerge(Employees);
                List<long> Ids = Employees.Select(x => x.Id).ToList();
                Employees = await UOW.EmployeeRepository.List(Ids);
                Logging.CreateAuditLog(Employees, new { }, nameof(EmployeeService));
                return Employees;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EmployeeService));
            }
            return null;
        }

        public async Task<int> Count(EmployeeFilter EmployeeFilter)
        {
            try
            {
                int result=await UOW.EmployeeRepository.Count(EmployeeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EmployeeService));
            }
            return 0;
        }

        public async Task<Employee> Create(Employee Employee)
        {
            if (!await EmployeeValidator.Create(Employee))
                return Employee;
            try
            {
                await UOW.EmployeeRepository.Create(Employee);
                Employee = (await UOW.EmployeeRepository.List(new List<long> { Employee.Id })).FirstOrDefault();
                Logging.CreateAuditLog(Employee, new { }, nameof(EmployeeService));
                return await UOW.EmployeeRepository.Get(Employee.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EmployeeService));
            }
            return null;
        }

        public async Task<Employee> Delete(Employee Employee)
        {
            if (!await EmployeeValidator.Delete(Employee))
                return Employee;
            try
            {
                await UOW.EmployeeRepository.Delete(Employee);
                Employee = (await UOW.EmployeeRepository.List(new List<long> { Employee.Id })).FirstOrDefault();
                Logging.CreateAuditLog(new { }, Employee, nameof(EmployeeService));
                return Employee;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EmployeeService));
            }
            return null;
        }

        public async Task<Employee> Get(long Id)
        {
            Employee Employee = await UOW.EmployeeRepository.Get(Id);
            if (Employee == null)
                return null;
            return Employee;
        }

        public async Task<List<Employee>> Import(List<Employee> Employees)
        {
            if (!await EmployeeValidator.Import(Employees))
                return Employees;
            try
            {
                await UOW.EmployeeRepository.BulkMerge(Employees);
                Employees = await UOW.EmployeeRepository.List(Employees.Select(x => x.Id).ToList());
                Logging.CreateAuditLog(new { }, Employees, nameof(EmployeeService));
                return Employees;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EmployeeService));
            }
            return null;
        }

        public async Task<List<Employee>> List(EmployeeFilter EmployeeFilter)
        {
            try
            {
                List<Employee> Employees = await UOW.EmployeeRepository.List(EmployeeFilter);
                return Employees;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EmployeeService));
            }
            return null;
        }

        public EmployeeFilter ToFilter(EmployeeFilter EmployeeFilter)
        {
            if (EmployeeFilter.OrFilter == null)
                EmployeeFilter.OrFilter = new List<EmployeeFilter>();
            return EmployeeFilter;
        }

        public async Task<Employee> Update(Employee Employee)
        {
            if (!await EmployeeValidator.Update(Employee))
                return Employee;
            try
            {
                var oldData = await UOW.EmployeeRepository.Get(Employee.Id);
                await UOW.EmployeeRepository.Update(Employee);
                Employee = (await UOW.EmployeeRepository.List(new List<long> { Employee.Id })).FirstOrDefault();
                Logging.CreateAuditLog(Employee, oldData, nameof(EmployeeService));
                return Employee;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EmployeeService));
            }
            return null;
        }
    }
}
