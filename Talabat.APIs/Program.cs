using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Helpers;
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
            var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String Not Found");
            #region Congifure Services


            builder.Services.AddControllers(); //allow dependancy injection of API Services
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //allow services if swagger 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); 
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });
            //builder.Services.AddScoped<IGenericRepository<>,GenericRepository<>>();
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>)); //allow dependency injection of genericRepo
            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            builder.Services.AddAutoMapper(typeof(MappingProfiles)); //easy syntax
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
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //Documentation of API to test API
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            #endregion


            app.MapControllers();
            app.UseStaticFiles();
            app.Run();
        }
    }

    internal class GenericReposotory<T>
    {
    }
}
