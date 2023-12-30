using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;


namespace Talabat.DAL.Identity
{
    public class AppIdentityDbContextSeeding
    {
        public static async Task SeedUserAsync(UserManager<User> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new User()
                {
                    DiplayName ="Ahmed Mohamed",
                    UserName ="aahmed",
                    Email ="aahmedfarhattt@gmail.com",
                    PhoneNumber ="01018451083",
                    Address = new Address()
                    {
                        FirstName ="Ahmed",
                        SecondName ="Mohamed",
                        Country ="Egypt",
                        City =  "El-Sinbellawein",
                        Street ="El mo3hda"
                    }
                };
                await userManager.CreateAsync(user, "P@ssw0rd");
            }
            
        }
    }
}
