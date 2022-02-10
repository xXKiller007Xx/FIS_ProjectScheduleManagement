using TrueSight.Common;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Group_3_BE.Entities
{
    public class TaskEmployeeMapping : DataEntity, IEquatable<TaskEmployeeMapping>
    {
        public long TaskId { get; set; }
        public long EmployeeId { get; set; }
        public Task Task { get; set; }
        public Employee Employee { get; set; }
        public bool Equals(TaskEmployeeMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class TaskEmployeeMappingFilter : FilterEntity
    {
        public IdFilter TaskId { get; set; }
        public IdFilter EmployeeId { get; set; }
        public List<TaskEmployeeMappingFilter> OrFilter { get; set; }
        public TaskEmployeeMappingOrder OrderBy { get; set; }
        public TaskEmployeeMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskEmployeeMappingOrder
    {
        Task = 0,
        Employee = 1,
    }

    [Flags]
    public enum TaskEmployeeMappingSelect : long
    {
        ALL = E.ALL,
        Task = E._0,
        Employee = E._1,
    }
}
