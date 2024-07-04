using Microsoft.Extensions.DependencyInjection;
using tablero_bi.Domain.Interfaces;
using tablero_bi.Infraestructure.Repositories;
namespace tablero_bi.Infraestructure
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            

            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IEmpresaRepository, EmpresaRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            return services;
        }


    }
}
