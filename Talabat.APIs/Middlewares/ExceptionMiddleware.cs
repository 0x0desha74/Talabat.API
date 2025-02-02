using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

                                                                     //Log Exception                        //Check Environment
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context) //When CLR execute this middleware, the method will be implemented
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                //Log Exception in Database

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() ?
                    new ApiExceptionResponse((int) HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace.ToString())
                    :new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);


                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var jsonResponse = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(jsonResponse);

            }
        }





    }
}
