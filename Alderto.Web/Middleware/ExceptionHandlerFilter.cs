using System.ComponentModel.DataAnnotations;
using Alderto.Domain.Exceptions;
using Discord.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alderto.Web.Middleware
{
    public class ExceptionHandlerFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                // Handle known API Exceptions.
                case DomainException domainException:
                    context.Result = domainException.ErrorState.StatusCode switch
                    {
                        ErrorStatusCode.BadRequest => new BadRequestObjectResult(new { domainException.Message }),
                        ErrorStatusCode.NotFound => new NotFoundObjectResult(new { domainException.Message }),
                        ErrorStatusCode.Forbidden => new ForbidResult(),
                        _ => throw context.Exception
                    };
                    return;

                case ValidationException validationException:
                    context.Result = new BadRequestObjectResult(new { validationException.Message });
                    return;

                case HttpException discordException:
                    context.Result = new BadRequestObjectResult(new
                    {
                        Message = discordException.DiscordCode switch
                        {
                            50001 => ErrorMessage.DISCORD_MISSING_PERMISSION_CHANNEL_READ,
                            50013 => ErrorMessage.DISCORD_MISSING_PERMISSION_CHANNEL_WRITE,
                            _ => throw context.Exception
                        }
                    });
                    return;
            }
        }
    }
}
