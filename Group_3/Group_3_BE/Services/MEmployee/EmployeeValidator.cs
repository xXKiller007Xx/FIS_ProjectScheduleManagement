using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MEmployee
{
    public interface IEmployeeValidator : IServiceScoped
    {
        Task<bool> Create(Employee Employee);
        Task<bool> Update(Employee Employee);
        Task<bool> Delete(Employee Employee);
        Task<bool> BulkMerge(List<Employee> Employees);
        Task<bool> BulkDelete(List<Employee> Employees);
        Task<bool> Import(List<Employee> Employees);
    }
    public class EmployeeValidator : IEmployeeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeExisted,
            CodeHasSpecialCharacter,
            CodeEmpty,
            NameEmpty,
            NameExisted,
            NameOverLength,
            JobEmpty,
            GenderEmpty,
            StatusEmpty,
            DateOfBirthEmpty,
            AddressEmpty,
            AddressOverLength,
            PhoneEmpty,
            PhoneUnderLength,
            PhoneOverLength,
            EmailEmpty,
            EmailOverLength,
            EmployeeInUsed,
        }
        private IUOW UOW;
        public EmployeeValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }
        public async Task<bool> ValidateId(Employee employee)
        {
            EmployeeFilter filter = new EmployeeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = employee.Id },
                Selects = EmployeeSelect.Id
            };
            int count = await UOW.EmployeeRepository.Count(filter);
            if (count == 0)
                employee.AddError(nameof(EmployeeValidator), nameof(employee), ErrorCode.IdNotExisted);
            return count == 1;
        }
        private async Task<bool> ValidateCode(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Code))
            {
                employee.AddError(nameof(EmployeeValidator), nameof(employee.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = employee.Code;
                if(employee.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(employee.Code))
                {
                    employee.AddError(nameof(EmployeeValidator), nameof(employee.Code), ErrorCode.CodeHasSpecialCharacter);
                }
                else
                {
                    EmployeeFilter EmployeeFilter = new EmployeeFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = employee.Id },
                        Code = new StringFilter { Equal = employee.Code },
                        Selects = EmployeeSelect.Code
                    };
                    int count = await UOW.EmployeeRepository.Count(EmployeeFilter);
                    if (count != 0)
                        employee.AddError(nameof(EmployeeValidator), nameof(employee.Code), ErrorCode.CodeExisted);
                }
            }
            return employee.IsValidated;
        }
        private async Task<bool> ValidateName(Employee Employee)
        {
            if (string.IsNullOrWhiteSpace(Employee.Name))
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Name), ErrorCode.NameEmpty);
            }
            else if (Employee.Name.Length > 500)
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Name), ErrorCode.NameOverLength);
            }
            return Employee.IsValidated;
        }
        private async Task<bool> ValidateGenderId(Employee Employee)
        {
            if (GenderEnum.MALE.Id != Employee.GenderId && GenderEnum.FEMALE.Id != Employee.GenderId && GenderEnum.OTHER.Id != Employee.GenderId)
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Gender), ErrorCode.GenderEmpty);
            return Employee.IsValidated;
        }
        private async Task<bool> ValidateJobId(Employee Employee)
        {
            if
            (
                JobEnum.LEADER.Id != Employee.JobId
                && JobEnum.EMPLOYEE.Id != Employee.JobId
                && JobEnum.MANAGER.Id != Employee.JobId
                && JobEnum.DIRECTOR.Id != Employee.JobId
            )
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Job), ErrorCode.JobEmpty);
            return Employee.IsValidated;
        }
        private async Task<bool> ValidateStatusId(Employee Employee)
        {
            if
            (
                StatusEnum.ACTIVE.Id != Employee.StatusId
                && StatusEnum.INACTIVE.Id != Employee.StatusId
            )
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Status), ErrorCode.StatusEmpty);
            return Employee.IsValidated;
        }
        private async Task<bool> ValidateDateOfBirth(Employee Employee)
        {
            if (string.IsNullOrWhiteSpace(Employee.DateOfBirth.ToString()))
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.DateOfBirth), ErrorCode.DateOfBirthEmpty);
            }
            return Employee.IsValidated;
        }
        private async Task<bool> ValidateAddress(Employee Employee)
        {
            if (string.IsNullOrWhiteSpace(Employee.Address))
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Address), ErrorCode.AddressEmpty);
            }
            else if (Employee.Address.Length > 500)
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Address), ErrorCode.AddressOverLength);
            }
            return Employee.IsValidated;
        }
        private async Task<bool> ValidatePhone(Employee Employee)
        {
            if (string.IsNullOrWhiteSpace(Employee.Phone))
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Phone), ErrorCode.PhoneEmpty);
            }
            else if (Employee.Phone.Length > 10)
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Phone), ErrorCode.PhoneOverLength);
            }
            else if (Employee.Phone.Length < 10)
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Phone), ErrorCode.PhoneUnderLength);
            }
            return Employee.IsValidated;
        }
        private async Task<bool> ValidateEmail(Employee Employee)
        {
            if (string.IsNullOrWhiteSpace(Employee.Email))
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Email), ErrorCode.EmailEmpty);
            }
            else if (Employee.Email.Length > 500)
            {
                Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Email), ErrorCode.EmailOverLength);
            }
            return Employee.IsValidated;
        }
        public async Task<bool> BulkDelete(List<Employee> Employees)
        {
            foreach (Employee Employee in Employees)
            {
                await Delete(Employee);
            }
            return Employees.All(e => e.IsValidated);
        }

        public async Task<bool> BulkMerge(List<Employee> Employees)
        {
            return true;
        }

        public async Task<bool> Create(Employee Employee)
        {
            await ValidateCode(Employee);
            await ValidateName(Employee);
            await ValidateGenderId(Employee);
            await ValidateJobId(Employee);
            await ValidateDateOfBirth(Employee);
            await ValidateAddress(Employee);
            await ValidatePhone(Employee);
            await ValidateEmail(Employee);
            await ValidateStatusId(Employee);
            return Employee.IsValidated;
        }

        public async Task<bool> Delete(Employee Employee)
        {
            if(await ValidateId(Employee))
            {
                if (Employee.Used)
                    Employee.AddError(nameof(EmployeeValidator), nameof(Employee.Id), ErrorCode.EmployeeInUsed);
            }

            return Employee.IsValidated;
        }

        public async Task<bool> Import(List<Employee> Employees)
        {
            foreach(Employee Employee in Employees)
            {
                await ValidateCode(Employee);
                await ValidateName(Employee);
                await ValidateStatusId(Employee);
                await ValidateAddress(Employee);
                await ValidateDateOfBirth(Employee);
                await ValidatePhone(Employee);
                await ValidateEmail(Employee);
            }
            return Employees.Any(e => !e.IsValidated) ? false : true;
        }

        public async Task<bool> Update(Employee Employee)
        {
            if(await ValidateId(Employee))
            {
                await ValidateCode(Employee);
                await ValidateName(Employee);
                await ValidateGenderId(Employee);
                await ValidateJobId(Employee);
                await ValidateDateOfBirth(Employee);
                await ValidateAddress(Employee);
                await ValidatePhone(Employee);
                await ValidateEmail(Employee);
                await ValidateStatusId(Employee);
            }
            return Employee.IsValidated;
        }
    }
}
