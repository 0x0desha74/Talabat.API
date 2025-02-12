using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
   public class AppIdentityDbContextSeed
    {

        public static async Task SeedUserAsync(UserManager<AppUser> userManager )
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                   DisplayName = "Mustafa Elsayed",
                   Email = "mustafa.elsayed@gmail.com",
                   UserName = "mustafa.elsayed",
                   PhoneNumber = "01123034045"
                };
                await userManager.CreateAsync(user, "P@ssword123");
            }
        }
    }
}
