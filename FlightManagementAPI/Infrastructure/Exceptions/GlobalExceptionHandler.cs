using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace FlightManagementAPI.Infrastructure.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (int statusCode, string errorMessage) = exception switch
            {
                ForbidException forbidException => (403, forbidException.Message),
                BadRequestException badRequestException => (400, badRequestException.Message),
                NotFoundException notFoundException => (404, notFoundException.Message),
                UnauthorizedException unauthorizedException => (401, unauthorizedException.Message),
                AccountException accountException => (400, accountException.Message),
                ValidationException validationException => (400, validationException.Message),
                DatabaseOperationException databaseOperationException => (500, databaseOperationException.Message),
                _ => (500, "Something went wrong...")
            };

            _logger.LogError(exception, exception.Message);

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(errorMessage);

            return true;
        }
    }
}
