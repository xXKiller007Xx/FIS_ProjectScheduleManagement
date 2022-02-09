using Group_3_BE.Entities;
using TrueSight.Common;

namespace Group_3_BE.Rpc.employee
{
    public class Employee_GenderDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public Employee_GenderDTO() { }
        public Employee_GenderDTO(Gender Gender)
        {

            this.Id = Gender.Id;

            this.Code = Gender.Code;

            this.Name = Gender.Name;

            this.Errors = Gender.Errors;
        }
    }

    public class Employee_GenderFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}
