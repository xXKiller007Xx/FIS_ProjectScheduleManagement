using System;
using System.Collections.Generic;

namespace Group_3_BE.Models
{
    public partial class JobDAO
    {
        public JobDAO()
        {
            Employees = new HashSet<EmployeeDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeeDAO> Employees { get; set; }
    }
}
