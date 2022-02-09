using Group_3_BE.Entities;
using TrueSight.Common;

namespace Group_3_BE.Rpc.employee
{
    public class Employee_JobDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public Employee_JobDTO() { }
        public Employee_JobDTO(Job Job)
        {

            this.Id = Job.Id;

            this.Code = Job.Code;

            this.Name = Job.Name;

            this.Errors = Job.Errors;
        }
    }

    public class Employee_JobFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}
