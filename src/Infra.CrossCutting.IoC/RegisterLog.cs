using Infra.CrossCutting.Log;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC
{
    /// <summary>
    /// Register DI Log
    /// </summary>
    public static class RegisterLog
    {
        /// <summary>
        /// Extension to register the services
        /// </summary>
        /// <param name="services"></param>
        public static void ServicesLog(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
        }
    }
}
