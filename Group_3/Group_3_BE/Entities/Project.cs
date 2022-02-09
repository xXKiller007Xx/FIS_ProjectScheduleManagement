using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace Group_3_BE.Entities
{
    public class Project : DataEntity, IEquatable<Project>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int Percentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? Used { get; set; }
        public long? StatusId { get; set; }

        public List<Task> Tasks { get; set; }
        public Status Status { get; set; }
        public bool Equals(Project other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ProjectFilter : FilterEntity
    {

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Description = 3,
        StartDate = 4,
        FinishDate = 5,
        Percentage = 6,
        UpdatedAt = 9,
        Status = 10,
    }
}
