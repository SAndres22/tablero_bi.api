namespace tablero_bi.api.Extensions
{
    public static class AuthorizationPolicyExtensions
    {
        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("ADMIN", "SUPERUSUARIO"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("USER", "SUPERUSUARIO", "ADMIN"));
                options.AddPolicy("RequireSuperUserRole", policy =>
                {
                    policy.RequireRole("SUPERUSUARIO");
                });
            });
        }
    }
}
