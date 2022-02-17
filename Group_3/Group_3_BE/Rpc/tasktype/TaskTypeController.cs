using Group_3_BE.Entities;
using Group_3_BE.Services.MProject;
using Group_3_BE.Services.MTaskType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Rpc.tasktype
{
    public class TaskTypeController : RpcController
    {
        private ITaskTypeService TaskTypeService;

        public TaskTypeController(ITaskTypeService TaskTypeService)
        {
            this.TaskTypeService = TaskTypeService;
        }

        [Route(TaskTypeRoute.Test), HttpGet]
        public async Task<ActionResult<string>> Test()
        {
            return "hello";
        }
    }
}
