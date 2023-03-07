using Chat.Bot.API.Hubs;
using Domain.Core.CnnStrings;
using Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Chat.Bot.API
{
    public class Program
    {
        const string CorsName = "chatBotAPI";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.ServicesLog();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();
            builder.Services.AddSignalR();

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();

            ConnectionStrings cnnStrings = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
            builder.Services.AddSingleton(cnnStrings);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CorsName,
                                  builder => builder.SetIsOriginAllowed(origin => true)
                                                    .AllowAnyMethod()
                                                    .AllowAnyHeader()
                                                    .AllowCredentials());
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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapHealthChecks("/check");

            app.MapControllers();

            app.MapHub<ChatBotHub>("/chat");

            app.UseCors(CorsName);

            app.Run();
        }
    }
}