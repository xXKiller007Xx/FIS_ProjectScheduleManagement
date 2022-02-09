using Group_3_BE.Services.MEmployee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Group_3_BE.Rpc.employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : RpcController
    {
        private IEmployeeService EmployeeService;
        public EmployeeController(IEmployeeService EmployeeService)
        {
            this.EmployeeService = EmployeeService;
        }

        [Route("/test"), HttpGet]
        public async Task<ActionResult<string>> Test()
        {
            return "hello";
        }
    }
}
