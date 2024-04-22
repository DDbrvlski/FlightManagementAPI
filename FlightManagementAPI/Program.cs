using FlightManagementAPI.Helpers.BaseService;
using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Interfaces.Helpers;
using FlightManagementAPI.Interfaces.Services.Airports;
using FlightManagementAPI.Interfaces.Services.Auths;
using FlightManagementAPI.Interfaces.Services.Flights;
using FlightManagementAPI.Interfaces.Services.Users;
using FlightManagementAPI.Services.Airports;
using FlightManagementAPI.Services.Auths;
using FlightManagementAPI.Services.Flights;
using FlightManagementAPI.Services.Users;
using FlightManagementData.Data;
using FlightManagementData.Models.Accounts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FlightManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<FlightManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("FlightManagementContext") ?? throw new InvalidOperationException("Connection string 'FlightManagementContext' not found.")));

            builder.Configuration.AddJsonFile("appsettings.json");

            var audiences = builder.Configuration.GetSection("Audiences").Get<Dictionary<string, string>>();

            builder.Services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
            builder.Services.AddTransient<IFlightService, FlightService>();
            builder.Services.AddTransient<IAirportService, AirportService>();
            builder.Services.AddTransient<IUserContextService, UserContextService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAuthSerivce, AuthSerivce>();

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
                .AddEntityFrameworkStores<FlightManagementContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudiences = builder.Configuration.GetSection("JWTKey:ValidAudience").Get<string[]>(),
                        ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]))
                    };
                });

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BookStoreAPI", Version = "v1" });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                  Enter 'Bearer' [space] and then your token in the text input below. 
                  Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                  new OpenApiSecurityScheme
                  {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                });
            });

            var app = builder.Build();
            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
