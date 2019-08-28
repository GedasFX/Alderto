using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [ApiController, Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult Forbid(string message)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                Message = message
            });
        }

        protected IActionResult BadRequest(string message)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new
            {
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
    }
}