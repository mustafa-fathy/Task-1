using Common;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var error = new ErrorDto
                {
                    Code = 500,
                    Message = "An unexpected error occurred",
                    Detailize = ex.Message
                };

                var result = JsonSerializer.Serialize(ResponseDto<object>.Failure(error));
                await context.Response.WriteAsync(result);
            }
        }
    }
}
