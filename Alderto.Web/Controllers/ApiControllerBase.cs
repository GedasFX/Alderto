using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult Forbid(string message)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ApiErrorResult
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = message
            });
        }

        protected IActionResult Content(object data)
        {
            return StatusCode(StatusCodes.Status200OK, data);
        }

        protected static class ForbidReason
        {
            public const string NotDiscordAdmin = "Could not confirm if user is an admin of the specified server.";
        }

        public class ApiErrorResult
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }
    }
}