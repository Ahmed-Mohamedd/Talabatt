using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Helpers;
using Talabat.API.Errors;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.Api.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services , IConfiguration Configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IResponseCachService, ResponseCachService>();


            services.AddAutoMapper(typeof(MappingProfile));
            services.Configure<JWT>(Configuration.GetSection("JWT")); //Add configuration For JWTSetting Class
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe")); //Add configuration For StripeSetting Class


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Count > 0)
                                                         .SelectMany(m => m.Value.Errors)
                                                         .Select(e => e.ErrorMessage).ToArray();
                    var ResponseMessage = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ResponseMessage);
                };

            });



            return services;
        }
    }
}
