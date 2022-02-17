using Group_3_BE.Services.MTask;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Rpc.task
{
    public class TaskController : RpcController
    {
        private ITaskService TaskService;
        
        public TaskController(ITaskService taskService)
        {
            this.TaskService = taskService;
        }

        [Route(TaskRoute.Test), HttpGet]
        public async Task<ActionResult<string>> Test()
        {
            return "hello";
        }
    }
}
