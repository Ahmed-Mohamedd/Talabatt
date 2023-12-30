using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.Api.Extensions;
using Talabat.Api.Helpers;
using Talabat.API.Middlewares;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.DAL.Identity;
using Talabat.Repository;
using Talabat.Repository.Context;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Service;

public class program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        #region Configure Services

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("TalabatConnection")));

        builder.Services.AddDbContext<AppIdentityDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));


        builder.Services.AddSingleton<IConnectionMultiplexer>(options => {
            var connection = builder.Configuration.GetConnectionString("RedisConnection");
            return ConnectionMultiplexer.Connect(connection);
        });

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration);
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("MyPolicy", MyPolicyOptions =>
            {
                MyPolicyOptions.AllowAnyHeader();
                MyPolicyOptions.AllowAnyMethod();
                MyPolicyOptions.AllowAnyOrigin();
            });
        });
        
        #endregion


        var app = builder.Build();

        #region Update Database

        //ApplicationDbContext dbContext = new ApplicationDbContext();
        //await dbContext.Database.MigrateAsync();

        
        using var scope = app.Services.CreateScope();     //Group of services where LifeTime is scoped
        var services = scope.ServiceProvider;            //services itself

        var LoggerFactory = services.GetRequiredService<ILoggerFactory>();

        try
        {
            //Ask CLR To Create Object From ApplicationDbContext Explicitly
            var DbContext = services.GetRequiredService<ApplicationDbContext>();
            var IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();
            var UserManager = services.GetRequiredService<UserManager<User>>();

            await DbContext.Database.MigrateAsync();
            await IdentityDbContext.Database.MigrateAsync();

            await ApplicationContextSeed.SeedAsync(DbContext , LoggerFactory);
            await AppIdentityDbContextSeeding.SeedUserAsync(UserManager);

        }
        catch (Exception ex)
        {
            var logger = LoggerFactory.CreateLogger<program>();
            logger.LogError(ex, "An Error Occurred While Updating Database");
        }

        #endregion

        #region  Configure the HTTP request pipeline.


        //Exception Middleware
        app.UseMiddleware<ExceptionMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseRouting();
        app.UseStaticFiles();

        app.UseHttpsRedirection();    // convert any protocol to https protocol for more security
        app.UseCors("MyPolicy");
        app.UseAuthentication();    
        app.UseAuthorization();

        app.MapControllers(); 
        #endregion

        app.Run();
    }
}

