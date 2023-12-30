using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entites.Identity;
using Talabat.Repository.Identity;

namespace Talabat.Api.Extensions
{
    public static  class IdentityServicesExtensions
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                   .AddJwtBearer(o =>
                   {
                       o.RequireHttpsMetadata=false;
                       o.SaveToken=false;
                       o.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuerSigningKey = true,
                           ValidateAudience=true,
                           ValidateIssuer=true,
                           ValidateLifetime=true,
                           ValidIssuer=Configuration["Jwt:Issuer"],
                           ValidAudience=Configuration["Jwt:Audience"],
                           IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))

                       };

                   });


            return services;
        }
    }
}
