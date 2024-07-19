using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace tablero_bi.api.Attributes
{
    public class CheckEmpresaAttribute : TypeFilterAttribute
    {
        public CheckEmpresaAttribute() : base(typeof(CheckEmpresaFilter))
        { }

        private class CheckEmpresaFilter : IAuthorizationFilter
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CheckEmpresaFilter(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;
                if (user.IsInRole("SUPERUSUARIO"))
                {
                    return; // Permitir acceso sin restricciones
                }

                var nitEmpresaClaim = user.FindFirst("nitEmpresa")?.Value;
                var nitEmpresaQuery = context.HttpContext.Request.Query["nitEmpresa"].ToString();

                if (string.IsNullOrEmpty(nitEmpresaClaim) || nitEmpresaClaim != nitEmpresaQuery)
                {
                    context.Result = new ForbidResult(); // No autorizado
                    return;
                }

            }
        }
    }



}
