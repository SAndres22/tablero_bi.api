using Microsoft.Extensions.DependencyInjection;

namespace tablero_bi.Application
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
