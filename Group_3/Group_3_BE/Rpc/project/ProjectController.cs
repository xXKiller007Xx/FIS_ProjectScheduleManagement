using Group_3_BE.Entities;
using Group_3_BE.Services.MProject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Rpc.project
{
    public class ProjectController : RpcController
    {
        private IProjectService ProjectService;
        public ProjectController(IProjectService ProjectService)
        {
            this.ProjectService = ProjectService;
        }

        [Route(ProjectRoute.Test), HttpGet]
        public async Task<ActionResult<string>> Test()
        {
            return "hello";
        }

        [Route(ProjectRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Project_ProjectFilterDTO Project_ProjectFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProjectFilter ProjectFilter = ConvertFilterDTOToFilterEntity(Project_ProjectFilterDTO);
            ProjectFilter = await ProjectService.ToFilter(ProjectFilter);
            int count = await ProjectService.Count(ProjectFilter);
            return count;
        }

        [Route(ProjectRoute.List), HttpPost]
        public async Task<ActionResult<List<Project_ProjectDTO>>> List([FromBody] Project_ProjectFilterDTO Project_ProjectFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProjectFilter ProjectFilter = ConvertFilterDTOToFilterEntity(Project_ProjectFilterDTO);
            ProjectFilter = await ProjectService.ToFilter(ProjectFilter);
            List<Project> Projects = await ProjectService.List(ProjectFilter);
            List<Project_ProjectDTO> Project_ProjectDTOs = Projects
                .Select(c => new Project_ProjectDTO(c)).ToList();
            return Project_ProjectDTOs;
        }

        [Route(ProjectRoute.Get), HttpPost]
        public async Task<ActionResult<Project_ProjectDTO>> Get([FromBody] Project_ProjectDTO Project_ProjectDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Project_ProjectDTO.Id))
                return Forbid();

            Project Project = await ProjectService.Get(Project_ProjectDTO.Id);
            return new Project_ProjectDTO(Project);
        }

        [Route(ProjectRoute.Create), HttpPost]
        public async Task<ActionResult<Project_ProjectDTO>> Create([FromBody] Project_ProjectDTO Project_ProjectDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Project_ProjectDTO.Id))
                return Forbid();

            Project Project = ConvertDTOToEntity(Project_ProjectDTO);
            Project = await ProjectService.Create(Project);
            Project_ProjectDTO = new Project_ProjectDTO(Project);
            if (Project.IsValidated)
                return Project_ProjectDTO;
            else
                return BadRequest(Project_ProjectDTO);
        }

        [Route(ProjectRoute.Update), HttpPost]
        public async Task<ActionResult<Project_ProjectDTO>> Update([FromBody] Project_ProjectDTO Project_ProjectDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Project_ProjectDTO.Id))
                return Forbid();

            Project Project = ConvertDTOToEntity(Project_ProjectDTO);
            Project = await ProjectService.Update(Project);
            Project_ProjectDTO = new Project_ProjectDTO(Project);
            if (Project.IsValidated)
                return Project_ProjectDTO;
            else
                return BadRequest(Project_ProjectDTO);
        }

        [Route(ProjectRoute.Delete), HttpPost]
        public async Task<ActionResult<Project_ProjectDTO>> Delete([FromBody] Project_ProjectDTO Project_ProjectDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Project_ProjectDTO.Id))
                return Forbid();

            Project Project = ConvertDTOToEntity(Project_ProjectDTO);
            Project = await ProjectService.Delete(Project);
            Project_ProjectDTO = new Project_ProjectDTO(Project);
            if (Project.IsValidated)
                return Project_ProjectDTO;
            else
                return BadRequest(Project_ProjectDTO);
        }

        [Route(ProjectRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProjectFilter ProjectFilter = new ProjectFilter();
            ProjectFilter.Id = new IdFilter { In = Ids };
            ProjectFilter.Selects = ProjectSelect.Id;
            ProjectFilter.Skip = 0;
            ProjectFilter.Take = int.MaxValue;

            List<Project> Projects = await ProjectService.List(ProjectFilter);
            Projects = await ProjectService.BulkDelete(Projects);
            if (Projects.Any(x => !x.IsValidated))
                return BadRequest(Projects.Where(x => !x.IsValidated));
            return true;
        }

        [Route(ProjectRoute.BulkMerge), HttpPost]
        public async Task<ActionResult<bool>> BulkMerge([FromBody] List<Project_ProjectDTO> Project_ProjectDTOs)
        {
            return true;
        }

        private async Task<bool> HasPermission(long Id)
        {
            ProjectFilter ProjectFilter = new ProjectFilter();
            if(Id == 0)
            {

            }
            else
            {
                ProjectFilter.Id = new IdFilter { Equal = Id };
                int count = await ProjectService.Count(ProjectFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Project ConvertDTOToEntity(Project_ProjectDTO Project_ProjectDTO)
        {
            Project Project = new Project();
            Project.Id = Project_ProjectDTO.Id;
            Project.Code = Project_ProjectDTO.Code;
            Project.Name = Project_ProjectDTO.Name;
            Project.Description = Project_ProjectDTO.Description;
            Project.StartDate = Project_ProjectDTO.StartDate;
            Project.FinishDate = Project_ProjectDTO.FinishDate;
            Project.Percentage = Project_ProjectDTO.Percentage;
            Project.UpdatedAt = Project_ProjectDTO.UpdatedAt;
            Project.StatusId = Project_ProjectDTO.StatusId;
            Project.Status = Project_ProjectDTO.Status == null ? null : new Status
            {
                Id = Project_ProjectDTO.Status.Id,
                Code = Project_ProjectDTO.Status.Code,
                Name = Project_ProjectDTO.Status.Name,
            };
            return Project;
        }

        private ProjectFilter ConvertFilterDTOToFilterEntity(Project_ProjectFilterDTO Project_ProjectFilterDTO)
        {
            ProjectFilter ProjectFilter = new ProjectFilter();
            ProjectFilter.Selects = ProjectSelect.ALL;
            ProjectFilter.Skip = Project_ProjectFilterDTO.Skip;
            ProjectFilter.Take = Project_ProjectFilterDTO.Take;
            ProjectFilter.OrderBy = Project_ProjectFilterDTO.OrderBy;
            ProjectFilter.OrderType = Project_ProjectFilterDTO.OrderType;

            ProjectFilter.Id = Project_ProjectFilterDTO.Id;
            ProjectFilter.Code = Project_ProjectFilterDTO.Code;
            ProjectFilter.Name = Project_ProjectFilterDTO.Name;
            ProjectFilter.Description = Project_ProjectFilterDTO.Description;
            ProjectFilter.StartDate = Project_ProjectFilterDTO.StartDate;
            ProjectFilter.FinishDate = Project_ProjectFilterDTO.FinishDate;
            ProjectFilter.Percentage = Project_ProjectFilterDTO.Percentage;
            ProjectFilter.UpdatedAt = Project_ProjectFilterDTO.UpdatedAt;
            ProjectFilter.StatusId = Project_ProjectFilterDTO.StatusId;

            ProjectFilter.Search = Project_ProjectFilterDTO.Search;

            return ProjectFilter;
        }
    }
}
