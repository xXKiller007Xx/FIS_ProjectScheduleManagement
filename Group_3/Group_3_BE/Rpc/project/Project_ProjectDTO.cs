using Group_3_BE.Entities;
using System;
using TrueSight.Common;

namespace Group_3_BE.Rpc.project
{
    public class Project_ProjectDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int Percentage { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool? Used { get; set; }
        public long? StatusId { get; set; }
        public Project_StatusDTO Status { get; set; }
        public Project_ProjectDTO()
        {

        }

        public Project_ProjectDTO(Project Project)
        {
            this.Id = Project.Id;
            this.Code = Project.Code;
            this.Name = Project.Name;
            this.Description = Project.Description;
            this.StartDate = Project.StartDate;
            this.FinishDate = Project.FinishDate;
            this.Percentage = Project.Percentage;
            this.UpdatedAt = Project.UpdatedAt;
            this.Used = Project.Used;
            this.StatusId = Project.StatusId;
            this.Status = Project.Status == null ? null : new Project_StatusDTO(Project.Status);
            this.Errors = Project.Errors;
        }
    }

    public class Project_ProjectFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter FinishDate { get; set; }
        public LongFilter Percentage { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public IdFilter StatusId { get; set; }
        public ProjectOrder OrderBy { get; set; }
        public string Search { get; set; }
    }
}
