using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entites.Identity;

namespace Talabat.Api.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<User> FindWithAddressByEmailAsync(this UserManager<User> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(U => U.Address).SingleOrDefaultAsync(U => U.Email == email);
            return user;
        }
    }
}
