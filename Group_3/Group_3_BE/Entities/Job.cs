using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Group_3_BE.Entities
{
    public class Job : DataEntity, IEquatable<Job>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Equals(Job other)
        {
            return other != null && other.Id == Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    public class JobFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<JobFilter> OrFilter { get; set; }
        public JobOrder OrderBy { get; set; }
        public JobSelect Selects { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum JobOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }
    [Flags]
    public enum JobSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
    }
}
