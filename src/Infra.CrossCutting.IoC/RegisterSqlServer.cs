using Domain.Core.CnnStrings;
using Infra.Data.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infra.CrossCutting.IoC
{
    /// <summary>
    /// Register DI Sql Server
    /// </summary>
    public static class RegisterSqlServer
    {
        /// <summary>
        /// Extension to register the services
        /// </summary>
        /// <param name="services"></param>
        public static void ServicesSqlServer(this IServiceCollection services, ConnectionStrings connectionStrings)
        {
            services.AddDbContext<ChatBotContext>(options => 
                options.UseSqlServer(connectionStrings.CnnDB)
            );
        }

        public static void SetupDB(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            using var context = scope.ServiceProvider.GetService<ChatBotContext>();
            if (context is not null)
            {
                context.Database.Migrate();
            }

        }
    }
}
