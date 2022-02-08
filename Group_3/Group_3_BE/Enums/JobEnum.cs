using TrueSight.Common;
using System.Collections.Generic;

namespace Group_3_BE.Enums
{
    public class JobEnum
    {
        public static GenericEnum DIRECTOR = new GenericEnum { Id = 1, Code = "Director", Name = "Giám đốc" };
        public static GenericEnum MANAGER = new GenericEnum { Id = 2, Code = "Manager", Name = "Quản lí" };
        public static GenericEnum LEADER = new GenericEnum { Id = 3, Code = "Leader", Name = "Trưởng nhóm" };
        public static GenericEnum EMPLOYEE = new GenericEnum { Id = 4, Code = "Employee", Name = "Nhân viên" };
        public static List<GenericEnum> JobEnumList = new List<GenericEnum>()
        {
            DIRECTOR, MANAGER, LEADER, EMPLOYEE
        };
    }
}
