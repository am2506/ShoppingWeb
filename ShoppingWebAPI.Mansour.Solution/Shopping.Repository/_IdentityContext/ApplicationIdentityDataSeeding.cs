using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.IdentityModels;

namespace Shopping.Repository._IdentityContext
{
    public static class ApplicationIdentityDataSeeding
    {
        
        public static async Task seedUserAsync(UserManager<ApplicationUser> userManager)
        {

            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "ahmed@gmail.com",
                    DisplayName = "Ahmed Mansour",
                    PhoneNumber = "0111111156"
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
