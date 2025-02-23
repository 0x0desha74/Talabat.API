using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            #region Congifure Services


            builder.Services.AddControllers(); //allow dependency injection of API Services

            //Allow dbContext dependency injection
            builder.Services.AddDbContext<StoreContext>(options =>
            {
            var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String Not Found");
                options.UseSqlServer(ConnectionString);
            });


            //Allow dependency injection of AppIdentityDbContext 
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                var Connection = builder.Configuration.GetConnectionString("IdentityConnection") ?? throw new InvalidOperationException("Connection String Not Found");
                options.UseSqlServer(Connection);
            });


            //Allow redisDb dependency injection
            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var cs = builder.Configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Connection String Not Found"); ;

                return ConnectionMultiplexer.Connect(cs);
            });

            //Allow Dependency injection of CORS [Cros Origin Service]
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });

            });
            //Application Services
            builder.Services.AddSwaggerServices();
            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            #endregion
             
            var app = builder.Build();
         
            //Update Database & Data Seeding
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>(); //Ask Explicitly
                await dbContext.Database.MigrateAsync(); //Apply Migration
                await StoreContextSeed.SeedAsync(dbContext); //Seeding Data 

                var IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>(); //Ask Explicitly for an object of AppIdentityDbContext
                await IdentityDbContext.Database.MigrateAsync(); //Apply Migration

                //Seed Data of the first user
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);

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
            //Add cross origin requests [Cross Origin Policy]
            app.UseCors("MyPolicy"); 
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            #endregion


            app.UseStaticFiles();
            app.MapControllers();
            app.Run();
        }
    }


}
