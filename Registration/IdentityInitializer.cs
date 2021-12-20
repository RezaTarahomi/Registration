using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Registration.Data;
using Registration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration
{
    public static class IdentityInitializer
    {   
        public static void SeedData(UserManager<User> userManager , IConfiguration configuration)
        {
            var users = configuration.GetSection("DefaultUsers").GetChildren().Select(user =>
           new ApiUser
           {
               FirstName = user["FirstName"],
               LastName = user["LastName"],
               UserName = user["UserName"],
               Password = user["Password"],
               Role = user["Role"]
           }).ToList();
            SeedUsers(userManager, users[0], users[0].Role);
            SeedUsers(userManager, users[1], users[1].Role);
            SeedUsers(userManager, users[2], users[2].Role);  
            }
        private static void SeedUsers(UserManager<User> userManager, UserDTO duser, string role)
        {
            if (userManager.FindByNameAsync(duser.UserName).Result == null)
            {
                User user = new User();
                user.UserName = duser.UserName;
                user.FirstName = duser.FirstName;
                user.LastName = duser.FirstName;

                IdentityResult result = userManager.CreateAsync(user, duser.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, role).Wait();
                }
            }
        }
    }


}
