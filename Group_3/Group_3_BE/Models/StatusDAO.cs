using System;
using System.Collections.Generic;

namespace Group_3_BE.Models
{
    public partial class StatusDAO
    {
        public StatusDAO()
        {
            Employees = new HashSet<EmployeeDAO>();
            Projects = new HashSet<ProjectDAO>();
            TaskTypes = new HashSet<TaskTypeDAO>();
            Tasks = new HashSet<TaskDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeeDAO> Employees { get; set; }
        public virtual ICollection<ProjectDAO> Projects { get; set; }
        public virtual ICollection<TaskTypeDAO> TaskTypes { get; set; }
        public virtual ICollection<TaskDAO> Tasks { get; set; }
    }
}
