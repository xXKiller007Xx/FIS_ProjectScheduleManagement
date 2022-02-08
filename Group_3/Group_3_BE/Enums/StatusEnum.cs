﻿using TrueSight.Common;
using System.Collections.Generic;

namespace Group_3_BE.Enums
{
    public class StatusEnum
    {
        public static GenericEnum ACTIVE = new GenericEnum { Id = 1, Code = "Active", Name = "Hoạt động" };
        public static GenericEnum INACTIVE = new GenericEnum { Id = 0, Code = "Inactive", Name = "Dừng hoạt động" };
        public static List<GenericEnum> StatusEnumList = new List<GenericEnum>
        {
            INACTIVE, ACTIVE
        };
    }
}
