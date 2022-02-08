using TrueSight.Common;
using System.Collections.Generic;

namespace Group_3_BE.Enums
{
    public class GenderEnum
    {
        public static GenericEnum MALE = new GenericEnum { Id = 1, Code = "Male", Name = "Nam" };
        public static GenericEnum FEMALE = new GenericEnum { Id = 2, Code = "Female", Name = "Nữ" };
        public static GenericEnum OTHER = new GenericEnum { Id = 3, Code = "Other", Name = "Khác" };
        public static List<GenericEnum> GenderEnumList = new List<GenericEnum>
        {
            MALE, FEMALE, OTHER
        };
    }
}
