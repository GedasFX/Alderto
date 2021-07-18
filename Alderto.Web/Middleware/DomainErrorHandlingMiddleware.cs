using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using Alderto.Domain.Exceptions;
using Discord.Net;
using Microsoft.AspNetCore.Http;

namespace Alderto.Web.Middleware
{
    public class DomainErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly JsonSerializerOptions _jsonSerializerOptions =
            new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public DomainErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    // Handle known API Exceptions.
                    case DomainException domainException:
                        context.Response.OnStarting(() =>
                        {
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = (int) domainException.ErrorState.StatusCode;

                            return Task.CompletedTask;
                        });

                        await context.Response.WriteAsync(
                            JsonSerializer.Serialize(
                                new { domainException.ErrorState, domainException.Message },
                                _jsonSerializerOptions));
                        break;

                    case ValidationException validationException:
                        context.Response.OnStarting(() =>
                        {
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;

                            return Task.CompletedTask;
                        });

                        await context.Response.WriteAsync(
                            JsonSerializer.Serialize(
                                new
                                {
                                    ErrorState = new ErrorState(ErrorStatusCode.BadRequest),
                                    validationException.Message
                                },
                                _jsonSerializerOptions));
                        break;

                    case HttpException discordException:
                        context.Response.OnStarting(() =>
                        {
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;

                            return Task.CompletedTask;
                        });

                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            ErrorState = new ErrorState(ErrorStatusCode.BadRequest),
                            Message = discordException.DiscordCode switch
                            {
                                50001 => ErrorMessage.DISCORD_MISSING_PERMISSION_CHANNEL_READ,
                                50013 => ErrorMessage.DISCORD_MISSING_PERMISSION_CHANNEL_WRITE,
                                _ => throw e
                            }
                        }, _jsonSerializerOptions));
                        break;

                    default:
                        throw;
                }
            }
        }
    }
}
