using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Alderto.Web.Middleware
{
    public class ValidatePermissions
    {
        private readonly RequestDelegate _next;

        public ValidatePermissions(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);

            throw new System.NotImplementedException();
        }
    }
}
