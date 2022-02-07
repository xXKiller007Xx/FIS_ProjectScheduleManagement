using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Group_3_BE.Entities
{
    public class Gender : DataEntity, IEquatable<Gender>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Equals(Gender other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    public class GenderFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<GenderFilter> OrFilter { get; set; }
        public GenderOrder OrderBy { get; set; }
        public GenderSelect Selects { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GenderOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }
    [Flags]
    public enum GenderSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
    }
}
