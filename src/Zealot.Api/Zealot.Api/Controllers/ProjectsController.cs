using Microsoft.AspNetCore.Mvc;
using Zealot.Api.ApiHelpers;
using Zealot.Domain.Models;
using Zealot.Services;

namespace Zealot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateProject([FromBody] ProjectModel model)
        {
            var result = _projectService.CreateProject(model);
            return result.ToActionResult();
        }
        [HttpPut]
        [Route("")]
        public IActionResult UpdateProject([FromBody] ProjectModel model)
        {
            var result = _projectService.UpdateProject(model);
            return result.ToActionResult();
        }
    }
}