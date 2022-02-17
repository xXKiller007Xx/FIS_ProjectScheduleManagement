using Group_3_BE.Entities;
using TrueSight.Common;

namespace Group_3_BE.Rpc.task
{
    public class Task_StatusDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Task_StatusDTO()
        {

        }

        public Task_StatusDTO(Status Status)
        {
            this.Id = Status.Id;

            this.Code = Status.Code;

            this.Name = Status.Name;

            this.Errors = Status.Errors;
        }
    }

    public class Task_StatusFilter : FilterDTO
    {
        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}
