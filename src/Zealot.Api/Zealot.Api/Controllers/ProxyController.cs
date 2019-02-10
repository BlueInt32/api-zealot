using System;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Zealot.Domain.Models;

namespace Zealot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        public IActionResult SendWiz([FromBody] WizModel model)
        {
            var client = new RestClient(model.EndpointUrl);
            var request = new RestRequest("/", (Method)Enum.Parse(typeof(Method), model.HttpMethod.ToString()));
            var response = client.Execute(request);
            return Ok($"{response.Content}");
        }
    }
}