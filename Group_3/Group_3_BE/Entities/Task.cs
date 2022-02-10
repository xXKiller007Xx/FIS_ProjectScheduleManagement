using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace Group_3_BE.Entities
{
    public class Task : DataEntity, IEquatable<Task>
    {
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

        public Project Project { get; set; }
        public TaskType TaskType { get; set; }
        public List<TaskEmployeeMapping> TaskEmployeeMappings { get; set; }
        public Status Status { get; set; }

        public bool Equals(Task other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class TaskFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter FinishDate { get; set; }
        public LongFilter Percentage { get; set; }
        public IdFilter ProjectId { get; set; }
        public IdFilter TaskTypeId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAd { get; set; }
        public IdFilter StatusId { get; set; }

        public List<TaskFilter> OrFilter { get; set; }
        public TaskOrder OrderBy { get; set; }
        public TaskSelect Selects { get; set; }
        public string Search { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Description = 3,
        StartDate = 4,
        FinishDate = 5,
        Percentage = 6,
        Project = 7,
        TaskType = 8,
        UpdatedAt = 9,
        Status = 10,
    }

    [Flags]
    public enum TaskSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Description = E._3,
        StartDate = E._4,
        FinishDate = E._5,
        Percentage = E._6,
        Project = E._7,
        TaskType = E._8,
        Status = E._9,
    }
}
