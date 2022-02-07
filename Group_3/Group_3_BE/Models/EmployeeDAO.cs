using System;
using System.Collections.Generic;

namespace Group_3_BE.Models
{
    public partial class EmployeeDAO
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? Used { get; set; }
        public long? StatusId { get; set; }

        public virtual GenderDAO Gender { get; set; }
        public virtual JobDAO Job { get; set; }
        public virtual StatuDAO Status { get; set; }
    }
}
