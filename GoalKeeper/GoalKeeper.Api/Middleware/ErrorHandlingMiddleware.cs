
using System.Net;
using System.Text.Json;
using GoalKeeper.Model.Exceptions;

namespace GoalKeeper.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlingMiddleware> logger)
        {
            context.Response.ContentType = "application/json";
 
            var (statusCode, customMessage) = exception switch
            {
                DuplicateUserException =>
                    (HttpStatusCode.Conflict, "Mail Id already exists."),
 
                ArgumentException =>
                    (HttpStatusCode.BadRequest, "Invalid input provided."),
 
                KeyNotFoundException =>
                    (HttpStatusCode.NotFound, "Mail Id not found."),
 
                UnauthorizedAccessException =>
                    (HttpStatusCode.Unauthorized, "You are not authorized to access this resource."),
 
                _ =>
                    (HttpStatusCode.InternalServerError, "Something went wrong. Please try again later.")
            };
 
            context.Response.StatusCode = (int)statusCode;
 
            logger.LogError( 
                "Request completed with StatusCode: {StatusCode} for Path: {Path}",
                context.Response.StatusCode,
                context.Request.Path
            );
 
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = customMessage,
                TimeStamp = DateTime.UtcNow
            };
 
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
