using System;
using System.Collections.Generic;

namespace Group_3_BE.Models
{
    public partial class TaskDAO
    {
        public TaskDAO()
        {
            TaskEmployeeMappings = new HashSet<TaskEmployeeMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int Percentage { get; set; }
        public long? ProjectId { get; set; }
        public long? TaskTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? Used { get; set; }
        public long? StatusId { get; set; }

        public virtual ProjectDAO Project { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual TaskTypeDAO TaskType { get; set; }
        public virtual ICollection<TaskEmployeeMappingDAO> TaskEmployeeMappings { get; set; }
    }
}
