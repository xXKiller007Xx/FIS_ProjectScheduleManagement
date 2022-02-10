using Group_3_BE.Entities;
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

        [Route("/count"), HttpGet]
        public async Task<ActionResult<string>> Test()
        {
            return "hello";
        }

        private EmployeeFilter ConvertFilterDTOToFilterEntity(Employee_EmployeeFilterDTO Employee_EmployeeFilterDTO)
        {
            EmployeeFilter EmployeeFilter = new EmployeeFilter();
            EmployeeFilter.Selects = EmployeeSelect.ALL;
            EmployeeFilter.Skip = EmployeeFilter.Skip;
            EmployeeFilter.Take = EmployeeFilter.Take;
            EmployeeFilter.OrderBy = EmployeeFilter.OrderBy;
            EmployeeFilter.OrderType = EmployeeFilter.OrderType;

            EmployeeFilter.Id = EmployeeFilter.Id;
            EmployeeFilter.Code = EmployeeFilter.Code;
            EmployeeFilter.Name = EmployeeFilter.Name;
            EmployeeFilter.JobId = EmployeeFilter.JobId;
            EmployeeFilter.GenderId = EmployeeFilter.GenderId;
            EmployeeFilter.DateOfBirth = EmployeeFilter.DateOfBirth;
            EmployeeFilter.Address = EmployeeFilter.Address;
            EmployeeFilter.Phone = EmployeeFilter.Phone;
            EmployeeFilter.Email = EmployeeFilter.Email;
            EmployeeFilter.CreatedAt = EmployeeFilter.CreatedAt;
            EmployeeFilter.UpdatedAt = EmployeeFilter.UpdatedAt;

            EmployeeFilter.Search = EmployeeFilter.Search;

            return EmployeeFilter;
        }
    }
}
