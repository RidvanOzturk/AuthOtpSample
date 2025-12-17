using System.Text.Json;

namespace AuthOtpSample.Api.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            if (httpContext.Response.HasStarted)
            {
                logger.LogError(ex, "Unhandled exception after response started. TraceId={TraceId}", httpContext.TraceIdentifier);
                throw;
            }

            var status = ex switch
            {
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                InvalidOperationException => StatusCodes.Status409Conflict,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            logger.LogError(ex, "Unhandled exception. Status={Status} TraceId={TraceId}", status, httpContext.TraceIdentifier);

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = status;
            httpContext.Response.Headers["X-Trace-Id"] = httpContext.TraceIdentifier;
            httpContext.Response.ContentType = "application/json";

            var payload = JsonSerializer.Serialize(new
            {
                error = ex.Message,
                traceId = httpContext.TraceIdentifier
            });

            await httpContext.Response.WriteAsync(payload);
        }
    }
}
