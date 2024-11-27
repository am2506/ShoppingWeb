using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RouteWebAPI.Config;
using RouteWebAPI.Helpers;
using RouteWebAPI.Helpers.HandleErrors;
using RouteWebAPI.Helpers.Middleware;
using StackExchange.Redis;
using System.Text;
using Shopping.Core.IdentityModels;
using Shopping.Core.IRepository;
using Shopping.Core.Models;
using Shopping.Repository;
using Shopping.Repository._IdentityContext;
using Shopping.Repository.Data;
using Shopping.Repository.DataSeeding;
using Shopping.Repository.IdentityContext;
namespace RouteWebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.
            builder.Services.AddControllers();
            //Add DbContext to the container
            builder.Services.AddDbContext<StoreDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Extension Method to Add Services and Repository
            builder.Services.RepositoryConfigurationEX();

            builder.Services.ServicesConfigurationEX();

            //Configure AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            //Configure InvalidModelStateResponseFactory to return custom error message
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actioncotext)
               =>
                {
                    var errors = actioncotext.ModelState
                     .Where(e => e.Value?.Errors.Count > 0)
                     .SelectMany(p => p.Value.Errors).
                      Select(p => p.ErrorMessage).ToArray();

                    var respone = new APIValidationErrorResponse()
                    {
                        Errors = errors ?? new string[] { "The input was invalid." }
                    };
                    return new BadRequestObjectResult(respone);
                };
            });

            //Allow DI for Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(Service =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("RedisConnection"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            //Configure Identity Database
            builder.Services.AddDbContext<ApplicationIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            //Add Identity Services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();
            //Configure Jwt Auth
            builder.Services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                var authkeybytes = Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"]);

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(authkeybytes),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Add CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader()
                    .AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseURL"]);
                });
            });

            #endregion

            var app = builder.Build();

            #region Get Scoped Serviecs and Update DataBase & Data Seeding  
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var _dbcontext = Services.GetRequiredService<StoreDbContext>();
            var _userManager = Services.GetRequiredService<UserManager<ApplicationUser>>();
            // To log Exceptaion by logger Factory
            var _LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                // ASk CLR for Creating Object from DBcontext Explicitly
                await _dbcontext.Database.MigrateAsync(); //update-Database
              //await StoreContextSeed.seedData(Context); // Data seeding
              //await StoreContextSeed.SeedDataGenrics<Product>(Context, "products.json");
              //await StoreContextSeed.SeedProducts(_dbcontext);
                await ApplicationIdentityDataSeeding.seedUserAsync(_userManager);
                await StoreContextSeed.SeedDeliveryMethods(_dbcontext);
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);

                //Log Exception
                var logger = _LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
            #endregion

            #region Kestrel Configureation & MiddleWare
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ErrorHandlerMiddleware>();
           // app.UseStatusCodePagesWithRedirects("/Errors");
           app.UseStatusCodePagesWithRedirects("/Errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("MyPolicy");

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
