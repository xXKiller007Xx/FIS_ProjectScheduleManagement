using System.ComponentModel;

namespace Group_3_BE.Rpc.project
{
    [DisplayName("Dự án")]
    public class ProjectRoute : Root
    {
        public const string Parent = Module + "/project";
        public const string Master = Module + "/project/project/project-master";
        public const string Detail = Module + "/project/project/project-detail/*";
        private const string Default = Rpc + Module + "/project";

        private const string ShortDefault = "/project";

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
