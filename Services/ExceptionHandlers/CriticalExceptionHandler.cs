using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ExamApp.Services.ExceptionHandlers
{
    public class CriticalExceptionHandler(ILogger<CriticalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is CriticalException)
            {
                logger.LogCritical(exception, "Critical error occurred: {Message}", exception.Message);
                var errorAsDto = ServiceResult.Fail("Unexpected server error occurred. We're working on it.", HttpStatusCode.InternalServerError);
                await httpContext.Response.WriteAsJsonAsync(errorAsDto, cancellationToken: cancellationToken);
            }

            return false;
        }
    }
}
