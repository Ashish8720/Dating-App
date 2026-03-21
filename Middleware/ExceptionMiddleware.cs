using Dating_App.Error;
using System.Net;

namespace Dating_App.Middleware
{
    /// <summary>
    /// Custom Middleware that handles exceptions occurring in the HTTP request pipeline and generates a JSON response.
    /// </summary>
    /// <remarks>This middleware intercepts unhandled exceptions, logs the error, and returns a standardized
    /// JSON response with an appropriate HTTP status code. The response includes additional details, such as the
    /// exception message and stack trace, when the application is running in the development environment.</remarks>
    /// <param name="next">The next middleware component in the request pipeline.</param>
    /// <param name="logger">The logger instance used to log exception details.</param>
    /// <param name="env">The hosting environment, used to determine whether the application is running in development mode.</param>
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger , IHostEnvironment env)
    {
        // The next middleware component in the pipeline
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Proceed to the next middleware component in the pipeline
                await next(context);
            }
            catch (Exception ex)
            {
                
                logger.LogError(ex, "{message}" , ex.Message);

                // Set the response content type and status code
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                // Determine the response based on the environment
                var response = env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, ex.Message ,  "Internal Server Error");

                // Serialize the response to JSON
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase };
                var json = System.Text.Json.JsonSerializer.Serialize(response, options);

                // Write the JSON response
                await context.Response.WriteAsync(json);
            }
        }
    }
}
