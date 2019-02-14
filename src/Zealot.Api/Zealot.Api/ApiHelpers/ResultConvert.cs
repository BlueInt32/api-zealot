using Microsoft.AspNetCore.Mvc;
using Zealot.Domain.Utilities;

namespace Zealot.Api.ApiHelpers
{
    public static class ResultConvert
    {
        public static IActionResult ToActionResult<T>(this OpResult<T> result) where T : class
        {
            if (!result.Success)
            {
                return new BadRequestObjectResult(result);
            }
            return new ObjectResult(result.Object);
        }
        public static IActionResult ToActionResult(this OpResult result)
        {
            if (!result.Success)
            {
                return new BadRequestObjectResult(result);
            }
            return new OkResult();
        }
    }
}
