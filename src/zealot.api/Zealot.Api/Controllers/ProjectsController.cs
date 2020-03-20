using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zealot.Api.ApiHelpers;
using Zealot.Domain;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;
using Zealot.Services;

namespace Zealot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            IProjectService projectService
            , ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult ListProjects()
        {
            var result = _projectService.ListProjects();
            return result.ToActionResult();
        }

        [HttpGet]
        [Route("{projectId}")]
        public IActionResult GetProject(Guid? projectId)
        {
            if (!projectId.HasValue)
            {
                return OpResult
                    .Bad(ErrorCode.PROJECT_ID_NOT_PROVIDED, $"Project id {projectId} was not recognized")
                    .ToActionResult();
            }
            var result = _projectService.GetProject(projectId.Value);
            return result.ToActionResult();
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateProject([FromBody] Project inputProject)
        {
            var result = _projectService.CreateProject(inputProject);
            return result.ToActionResult();
        }
        [HttpPut]
        [Route("{projectId}")]
        public IActionResult UpdateProject(Guid? projectId, [FromBody] Project inputProject)
        {
            /// TODO: use https://www.tutorialdocs.com/article/webapi-data-binding.html
            if (!inputProject.Id.HasValue)
            {
                inputProject.Id = projectId;
            }
            var result = _projectService.UpdateProject(inputProject);
            return result.ToActionResult();
        }
    }
}