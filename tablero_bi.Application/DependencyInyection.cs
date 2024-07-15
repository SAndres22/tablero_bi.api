using Microsoft.Extensions.DependencyInjection;
using tablero_bi.Application.Interfaces;
using tablero_bi.Application.Services;

namespace tablero_bi.Application
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            //Configuracion de Automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ICifradoService, CifradoService>();
            services.AddTransient<IEmpresaService, EmpresaService>();
            services.AddTransient<ISucursalService, SucursalService>();
            services.AddTransient<IUploadImagenService, UploadImagenService>();





            return services;
        }
    }
}
