using Alderto.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [ApiController, Authorize, Consumes("application/json")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult Content(object data)
        {
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}