using Alderto.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [ApiController, Authorize, Consumes("application/json")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult Forbid(ErrorMessage message)
        {
            return StatusCode(StatusCodes.Status403Forbidden, message);
        }

        protected IActionResult BadRequest(ErrorMessage message)
        {
            return StatusCode(StatusCodes.Status400BadRequest, message);
        }

        protected IActionResult NotFound(ErrorMessage message)
        {
            return StatusCode(StatusCodes.Status404NotFound, message);
        }

        protected IActionResult Content(object data)
        {
            return StatusCode(StatusCodes.Status200OK, data);
        }

        
    }
}