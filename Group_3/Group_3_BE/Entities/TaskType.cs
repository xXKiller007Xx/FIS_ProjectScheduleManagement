using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace Group_3_BE.Entities
{
    public class TaskType : DataEntity, IEquatable<TaskType>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? StatusId { get; set; }

        public List<Task> Tasks { get; set; }
        public Status Status { get; set; }

        public bool Equals(TaskType other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class TaskTypeFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAd { get; set; }
        public IdFilter StatusId { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskTypeOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Description = 3,
        UpdatedAt = 4,
        Status = 5,
    }

    [Flags]
    public enum TaskTypeSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Description = E._3,
        Status = E._4,
    }
}
