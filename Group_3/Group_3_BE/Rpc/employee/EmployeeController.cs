using Group_3_BE.Entities;
using Group_3_BE.Services.MEmployee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

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

        [HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Employee_EmployeeFilterDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            EmployeeFilter filter = ConvertFilterDTOToFilterEntity(dto);
            filter = EmployeeService.ToFilter(filter);
            int count = await EmployeeService.Count(filter);
            return count;
        }
        [HttpPost]
        public async Task<ActionResult<List<Employee_EmployeeDTO>>> List([FromBody] Employee_EmployeeFilterDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            EmployeeFilter filter = ConvertFilterDTOToFilterEntity(dto);
            filter = EmployeeService.ToFilter(filter);
            List<Employee> employees = await EmployeeService.List(filter);
            List<Employee_EmployeeDTO> E_E_DTOs = employees.Select(x => new Employee_EmployeeDTO(x)).ToList();
            return E_E_DTOs;
        }
        [HttpPost]
        public async Task<ActionResult<Employee_EmployeeDTO>> Get([FromBody] Employee_EmployeeDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(dto.Id))
                return Forbid();
            Employee employee = await EmployeeService.Get(dto.Id);
            return new Employee_EmployeeDTO(employee);
        }
        [HttpPost]
        public async Task<ActionResult<Employee_EmployeeDTO>> Create([FromBody] Employee_EmployeeDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(dto.Id))
                return Forbid();
            Employee employee = ConvertDTOToEntity(dto);
            employee = await EmployeeService.Create(employee);
            dto = new Employee_EmployeeDTO(employee);
            if (employee.IsValidated)
                return dto;
            else
                return BadRequest(dto);
        }
        [HttpPost]
        public async Task<ActionResult<Employee_EmployeeDTO>> Update([FromBody] Employee_EmployeeDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(dto.Id))
                return Forbid();
            Employee employee = ConvertDTOToEntity(dto);
            employee = await EmployeeService.Update(employee);
            dto = new Employee_EmployeeDTO(employee);
            if (employee.IsValidated)
                return dto;
            else
                return BadRequest(dto);
        }
        [HttpPost]
        public async Task<ActionResult<Employee_EmployeeDTO>> Delete([FromBody] Employee_EmployeeDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (!await HasPermission(dto.Id))
                return Forbid();
            Employee employee = ConvertDTOToEntity(dto);
            employee = await EmployeeService.Delete(employee);
            dto = new Employee_EmployeeDTO(employee);
            if (employee.IsValidated)
                return dto;
            else
                return BadRequest(dto);
        }
        [HttpPost]
        public async Task<ActionResult<Employee_EmployeeDTO>> BulkCreate([FromBody] List<Employee_EmployeeDTO> dtos)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            List<Employee> employees = dtos.Select(x => ConvertDTOToEntity(x)).ToList();
            employees = await EmployeeService.Import(employees);
            if (employees.Any(x => x.IsValidated == false))
                return BadRequest(employees);
            return Ok();
        }
        private async Task<bool> HasPermission(long Id)
        {
            EmployeeFilter filter = new EmployeeFilter();
            filter = EmployeeService.ToFilter(filter);
            if (Id != 0)
            {
                filter.Id = new IdFilter { Equal = Id };
                int count = await EmployeeService.Count(filter);
                if (count == 0)
                    return false;
            }
            return true;
        }
        private Employee ConvertDTOToEntity(Employee_EmployeeDTO dto)
        {
            Employee employee = new Employee();
            employee.Id = dto.Id;
            employee.Code = dto.Code;
            employee.Name = dto.Name;
            employee.JobId = dto.JobId;
            employee.GenderId = dto.GenderId;
            employee.Address = dto.Address;
            employee.DateOfBirth = dto.DateOfBirth;
            employee.StatusId = dto.StatusId;
            employee.Phone = dto.Phone;
            employee.Email = dto.Email;
            employee.Gender = dto.Gender == null ? null : new Gender
            {
                Id = dto.Gender.Id,
                Code = dto.Gender.Code,
                Name = dto.Gender.Name,
            };
            employee.Job = dto.Job == null ? null : new Job
            {
                Id = dto.Job.Id,
                Code = dto.Job.Code,
                Name = dto.Job.Name,
            };
            employee.Status = dto.Status == null ? null : new Status
            {
                Id = dto.Status.Id,
                Code = dto.Status.Code,
                Name = dto.Status.Name,
            };
            return employee;
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
