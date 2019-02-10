using Microsoft.AspNetCore.Mvc;
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
        public IActionResult SaveProject([FromBody] ProjectModel model)
        {
            _projectService.SaveProject(model);
            return Ok(1);
        }
    }
}