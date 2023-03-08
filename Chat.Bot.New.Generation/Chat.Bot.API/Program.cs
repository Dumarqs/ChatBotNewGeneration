using Application.Automapper;
using Chat.Bot.API.Hubs;
using Domain.Core.CnnStrings;
using Infra.CrossCutting.IoC;

namespace Chat.Bot.API
{
    public class Program
    {
        const string CorsName = "chatBotAPI";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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