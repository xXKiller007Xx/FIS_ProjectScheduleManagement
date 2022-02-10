using Group_3_BE.Entities;
using System;
using TrueSight.Common;

namespace Group_3_BE.Rpc.employee
{
    public class Employee_EmployeeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? JobId { get; set; }
        public long? GenderId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Used { get; set; }
        public long? StatusId { get; set; }

        public Employee_GenderDTO Gender { get; set; }
        public Employee_JobDTO Job { get; set; }
        public Employee_StatusDTO Status { get; set; }

        public Employee_EmployeeDTO() { }
        public Employee_EmployeeDTO(Employee Employee)
        {
            this.Id = Employee.Id;
            this.Code = Employee.Code;
            this.Name = Employee.Name;
            this.JobId = Employee.JobId;
            this.GenderId = Employee.GenderId;
            this.DateOfBirth = Employee.DateOfBirth;
            this.Address = Employee.Address;
            this.Phone = Employee.Phone;
            this.Email = Employee.Email;
            this.UpdatedAt = Employee.UpdatedAt;
            this.Used = Employee.Used;
            this.StatusId = Employee.StatusId;
            this.Gender = Employee.Gender == null ? null : new Employee_GenderDTO(Employee.Gender);
            this.Job = Employee.Job == null ? null: new Employee_JobDTO(Employee.Job);
            this.Status = Employee.Status == null ? null : new Employee_StatusDTO(Employee.Status);

            this.Errors = Employee.Errors;
        }
    }

    public class Employee_EmployeeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter JobId { get; set; }
        public IdFilter GenderId { get; set; }
        public DateFilter DateOfBirth { get; set; }
        public StringFilter Address { get; set; }
        public StringFilter Phone { get; set; }
        public StringFilter Email { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public IdFilter StatusId { get; set; }
        public EmployeeOrder OrderBy { get; set; }
        public string Search { get; set; }
    }
}
