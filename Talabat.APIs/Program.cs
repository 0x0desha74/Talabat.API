using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            #region Congifure Services
            var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String Not Found");


            builder.Services.AddControllers(); //allow dependency injection of API Services
          
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });


            builder.Services.AddSwaggerServices();
            builder.Services.AddApplicationServices();
            #endregion

            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>(); //Ask Explicitly
                await dbContext.Database.MigrateAsync(); //Apply Migration

                await StoreContextSeed.SeedAsync(dbContext); //Seeding Data 
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occurred During Appling the Migration");
            }


            #region Configure Kestrel MiddelWares 

            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            
            }


            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            #endregion


            app.UseStaticFiles();
            app.MapControllers();
            app.Run();
        }
    }


}
