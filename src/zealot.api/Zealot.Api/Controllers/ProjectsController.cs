using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zealot.Api.ApiHelpers;
using Zealot.Domain;
using Zealot.Domain.Models;
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

        /// <summary>
        /// Get a project. 
        /// Note that this uses <see cref="ProjectOutputFormatter"/> to populate types in JSON output
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
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
        public IActionResult CreateProject([FromBody] ProjectModel model)
        {
            var result = _projectService.CreateProject(model);
            return result.ToActionResult();
        }
        [HttpPut]
        [Route("{projectId}")]
        public IActionResult UpdateProject(Guid? projectId, [FromBody] ProjectModel model)
        {
            if (!model.Id.HasValue)
            {
                model.Id = projectId;
            }
            var result = _projectService.UpdateProject(model);
            return result.ToActionResult();
        }
    }
}