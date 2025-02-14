using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.ComponentModel.Design;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ITokenService), typeof(TokenService));
            
            ///
            ///Configure Identity System [who will represent User and Who will represent Role]
            ///Add Interfaces Containing Signature of methods like [CreateAsync()..]
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {

            }).AddEntityFrameworkStores<AppIdentityDbContext>(); //Implement this methods' signatures

            ///
            ///Allow Dependency Injection Of UserManager , RoleManager
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return services;
        }
    }
}
