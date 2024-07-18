using tablero_bi.Application.Interfaces;
using tablero_bi.Application.Services;

namespace tablero_bi.api.Extensions
{
    public static class LoggerServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerService>();
        }
    }
}
