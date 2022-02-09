using System;
using TrueSight.Common;

namespace Group_3_BE.Entities
{
    public class AuditLog : DataEntity
    {
        public long Id { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public string ModuleName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public DateTime Time { get; set; }
    }
}
