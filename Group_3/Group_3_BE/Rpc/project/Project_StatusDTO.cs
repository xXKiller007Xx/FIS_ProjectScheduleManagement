using Group_3_BE.Entities;
using TrueSight.Common;

namespace Group_3_BE.Rpc.project
{
    public class Project_StatusDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Project_StatusDTO()
        {

        }

        public Project_StatusDTO(Status Status)
        {
            this.Id = Status.Id;

            this.Code = Status.Code;

            this.Name = Status.Name;

            this.Errors = Status.Errors;
        }
    }

    public class Project_StatusFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}
