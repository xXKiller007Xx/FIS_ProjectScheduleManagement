﻿using Newtonsoft.Json;
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
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter FinishDate { get; set; }
        public LongFilter Percentage { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public IdFilter StatusId { get; set; }

        public List<ProjectFilter> OrFilter { get; set; }
        public ProjectOrder OrderBy { get; set; }
        public ProjectSelect Selects { get; set; }
        public string Search { get; set; }
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
        UpdatedAt = 7,
        Status = 8,
    }

    [Flags]
    public enum ProjectSelect: long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Description = E._3,
        StartDate = E._4,
        FinishDate = E._5,
        Percentage = E._6,
        Status = E._7,
    }
}
