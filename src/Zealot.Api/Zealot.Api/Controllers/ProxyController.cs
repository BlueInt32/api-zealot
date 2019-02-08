using Microsoft.AspNetCore.Mvc;
using Zealot.Api.Models;

namespace Zealot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        public IActionResult SendWiz([FromBody] WizModel model)
        {
            return Ok($"received wiz for {model.Endpoint}");
        }
    }
}