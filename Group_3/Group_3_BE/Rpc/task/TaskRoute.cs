using System.ComponentModel;

namespace Group_3_BE.Rpc.task
{
    [DisplayName("Công việc")]
    public class TaskRoute : Root
    {
        public const string Parent = Module + "/task";
        public const string Master = Module + "/task/task/task-master";
        public const string Detail = Module + "/task/task/task-detail/*";
        private const string Default = Rpc + Module + "/task";

        private const string ShortDefault = "/task";

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
