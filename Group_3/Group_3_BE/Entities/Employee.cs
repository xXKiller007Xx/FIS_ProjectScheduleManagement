using Group_3_BE.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TrueSight.Common;

namespace Group_3_BE.Entities
{
    public class Employee : DataEntity, IEquatable<Employee>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long JobId { get; set; }
        public long GenderId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public long StatusId { get; set; }
        public Status Status { get; set; }
        public Job Job { get; set; }
        public Gender Gender { get; set; }
        public bool Equals(Employee other)
        {
            return other!=null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    public class EmployeeFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter JobId { get; set; }
        public IdFilter GenderId { get; set; }
        public DateFilter DateOfBirth { get; set; }
        public StringFilter Address { get; set; }
        public StringFilter Phone { get; set; }
        public StringFilter Email { get; set; }
        public List<EmployeeFilter> OrFilter { get; set; }
        public EmployeeOrder OrderBy { get; set; }
        public EmployeeSelect Selects { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmployeeOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Job = 3,
        Gender = 4,
        DateOfBirth = 5,
        Address = 6,
        Phone = 7,
        Email = 8,
        Status = 9,
    }
    [Flags]
    public enum EmployeeSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Job = E._3,
        Gender = E._4,
        DateOfBirth = E._5,
        Address = E._6,
        Phone = E._7,
        Email = E._8,
        Status = E._9,
    }
}
