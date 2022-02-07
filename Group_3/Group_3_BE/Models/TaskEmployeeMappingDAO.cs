using System;
using System.Collections.Generic;

namespace Group_3_BE.Models
{
    public partial class TaskEmployeeMappingDAO
    {
        public long? TaskId { get; set; }
        public long? EmployeeId { get; set; }

        public virtual EmployeeDAO Employee { get; set; }
        public virtual TaskDAO Task { get; set; }
    }
}
