using Application.Automapper;
using Chat.Bot.API.Automapper;
using Chat.Bot.API.Hubs;
using Chat.Bot.API.Identity;
using Chat.Bot.API.Middlewares;
using Chat.Bot.API.Models;
using Chat.Bot.API.Models;
using Domain.Core.CnnStrings;
using Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Chat.Bot.API
{
    public class Program
    {
        const string CorsName = "chatBotAPI";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            ConnectionStrings cnnStrings = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
            builder.Services.ServicesSqlServer(cnnStrings);

            // Add services to the container.
            builder.Services.ServicesLog();
            builder.Services.ServicesApplication();
            builder.Services.ServicesRepository();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();
            builder.Services.AddSignalR();

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();            

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CorsName,
                                  builder => builder.SetIsOriginAllowed(origin => true)
                                                    .AllowAnyMethod()
                                                    .AllowAnyHeader()
                                                    .AllowCredentials());
            });

            builder.Services.AddAutoMapper(typeof(DomainToDtoMappingProfile));
            builder.Services.AddAutoMapper(typeof(DtoToDomainMappingProfile));
            builder.Services.AddAutoMapper(typeof(ViewModelToDtoMappingProfile));
            builder.Services.AddAutoMapper(typeof(DtoToViewModelMappingProfile));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddDefaultTokenProviders();

            builder.Services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            builder.Services.AddTransient<IRoleStore<IdentityRole>, RoleStore>();

            JwtConfigurations jwtOptions = builder.Configuration.GetSection("JwtParameters").Get<JwtConfigurations>();
            builder.Services.AddSingleton(jwtOptions);

            // Adding Authentication
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
                    ValidateAudience = false,
                    //ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHealthChecks("/check");

            app.MapControllers();

            app.MapHub<ChatBotHub>("/chat");

            app.UseCors(CorsName);

            app.Run();
        }
    }
}