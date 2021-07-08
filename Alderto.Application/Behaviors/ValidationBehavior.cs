using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Alderto.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var validationResult = new List<ValidationResult>();
            if (Validator.TryValidateObject(request, new ValidationContext(request), validationResult, true))
            {
                return await next();
            }

            throw new Exception(validationResult[0].ErrorMessage);
        }
    }
}