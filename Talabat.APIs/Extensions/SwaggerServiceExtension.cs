namespace Talabat.APIs.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //allow services if swagger 
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }


        public static IApplicationBuilder UseSwaggerMiddlewares(this WebApplication app)
        {
            //Documentation of API to test API
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
