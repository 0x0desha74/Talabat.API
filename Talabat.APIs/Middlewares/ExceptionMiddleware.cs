namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;


        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsunc(HttpContext context)
        {

        }





    }
}
