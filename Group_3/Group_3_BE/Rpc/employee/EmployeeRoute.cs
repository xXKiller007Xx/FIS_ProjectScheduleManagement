using System.ComponentModel;

namespace Group_3_BE.Rpc.employee
{
    [DisplayName("Nhân viên")]
    public class EmployeeRoute : Root
    {
        public const string Parent = Module + "/employee";
        public const string Master = Module + "/employee/employee/employee-master";
        public const string Detail = Module + "/employee/employee/employee-detail/*";
        private const string Default = Rpc + Module + "/employee";

        private const string ShortDefault = "/employee";

        public const string Count = ShortDefault + "/count";
        public const string List = ShortDefault + "/list";
        public const string Get = ShortDefault + "/get";
        public const string Create = ShortDefault + "/create";
        public const string Update = ShortDefault + "/update";
        public const string Delete = ShortDefault + "/delete";
        public const string BulkDelete = ShortDefault + "/bulk-delete";
        public const string BulkMerge = ShortDefault + "/bulk-merge";
        public const string BulkCreate = ShortDefault + "/bulk-create";

        public const string Test = ShortDefault + "/test";
    }
}
