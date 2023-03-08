using Domain.Repositories;
using Infra.Data.SqlServer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC
{
    public static class RegisterRepository
    {
        public static void ServicesRepository(this IServiceCollection services)
        {
            services.AddScoped<IRoomRepository, RoomRepository>();
        }
    }
}
