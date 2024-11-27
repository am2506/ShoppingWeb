using RouteWebAPI.Helpers.HandleErrors;
using System.Net;
using System.Text.Json;

namespace RouteWebAPI.Helpers.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                await _next.Invoke(httpcontext);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message); // Developement Environment
                // Log Exception in Production Environment in File or Database

                // Set Response 
                // 1. Set Response Type
                httpcontext.Response.ContentType = "application/json";
                // 2. Set Response Status Code
                httpcontext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                // 3. Set Response Body

                var response = _env.IsDevelopment() ?
                new APIExceptionResponse(StatusCodes.Status500InternalServerError,
                ex.Message, ex?.StackTrace?.ToString())
                :
                new APIExceptionResponse(StatusCodes.Status500InternalServerError);
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var JsonRes = JsonSerializer.Serialize(response,options);
                await httpcontext.Response.WriteAsync(JsonRes);
            }
        }
    }
}