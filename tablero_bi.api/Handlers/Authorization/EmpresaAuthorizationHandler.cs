using Microsoft.AspNetCore.Authorization;

namespace tablero_bi.api.Handlers.Authorization
{
    public class EmpresaAuthorizationHandler : AuthorizationHandler<EmpresaRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            EmpresaRequirement requirement)
        {
            // Si el usuario es un SUPERUSUARIO, permitir acceso sin restricciones
            if (context.User.IsInRole("SUPERUSUARIO"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var nitEmpresaClaim = context.User.FindFirst("nitEmpresa")?.Value;
            var nitEmpresaQuery = context.Resource as string; // Suponiendo que pasamos el nitEmpresa como recurso

            if (!string.IsNullOrEmpty(nitEmpresaClaim) && nitEmpresaClaim == nitEmpresaQuery)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
