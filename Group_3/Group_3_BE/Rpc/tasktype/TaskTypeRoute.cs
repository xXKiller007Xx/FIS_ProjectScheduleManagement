using System.ComponentModel;

namespace Group_3_BE.Rpc.tasktype
{
    [DisplayName("Loại công việc")]
    public class TaskTypeRoute : Root
    {
        public const string Parent = Module + "/task-type";
        public const string Master = Module + "/task-type/task-type/task-type-master";
        public const string Detail = Module + "/task-type/task-type/task-type-detail/*";
        private const string Default = Rpc + Module + "/task-type";

        private const string ShortDefault = "/task-type";

        public const string Count = ShortDefault + "/count";
        public const string List = ShortDefault + "/list";
        public const string Get = ShortDefault + "/get";
        public const string Create = ShortDefault + "/create";
        public const string Update = ShortDefault + "/update";
        public const string Delete = ShortDefault + "/delete";
        public const string BulkDelete = ShortDefault + "/bulk-delete";
        public const string BulkMerge = ShortDefault + "/bulk-merge";

        public const string Test = ShortDefault + "/test";
    }
}
