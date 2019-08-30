using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [ApiController, Authorize]
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

        protected IActionResult Content(object data)
        {
            return StatusCode(StatusCodes.Status200OK, data);
        }

        protected class ErrorMessage
        {
            public int Code { get; set; }
            public string Message { get; set; }

            public ErrorMessage(int code, string message)
            {
                Code = code;
                Message = message;
            }
        }

        protected static class ErrorMessages
        {
            public static ErrorMessage NotDiscordAdmin { get; } =
                new ErrorMessage(100, "Could not confirm if user is an admin of the specified server.");

            public static ErrorMessage BankDoesNotExist { get; } =
                new ErrorMessage(200, "The given bank was not found.");

            public static ErrorMessage BankNameAlreadyExists { get; } =
                new ErrorMessage(201, "A bank with the given name already exists.");

        }
    }
}