using Microsoft.AspNetCore.Authorization;
using tablero_bi.api.Handlers.Authorization;

namespace tablero_bi.api.Extensions
{
    public static class AuthorizationPolicyExtensions
    {
        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Políticas basadas en roles
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("ADMIN", "SUPERUSUARIO"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("USER", "SUPERUSUARIO", "ADMIN"));
                options.AddPolicy("RequireSuperUserRole", policy => policy.RequireRole("SUPERUSUARIO"));

                // Política basada en recursos para verificar nitEmpresa
                options.AddPolicy("CheckEmpresaPolicy", policy =>
                {
                    policy.Requirements.Add(new EmpresaRequirement());
                });
            });

            services.AddSingleton<IAuthorizationHandler, EmpresaAuthorizationHandler>();
        }


    }
}
