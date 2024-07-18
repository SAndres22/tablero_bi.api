using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.api.Attributes
{
    public class CheckEmpresaAttribute : TypeFilterAttribute
    {
        public CheckEmpresaAttribute() : base(typeof(CheckEmpresaFilter))
        {}

        private class CheckEmpresaFilter : IAuthorizationFilter
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ISucursalRepository _sucursalRepository;

            public CheckEmpresaFilter(IHttpContextAccessor httpContextAccessor,
                ISucursalRepository sucursalRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _sucursalRepository = sucursalRepository;
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
                var idSucursalQuery = context.HttpContext.Request.Query["idSucursal"].ToString();

                if (!int.TryParse(idSucursalQuery, out int idSucursal))
                {
                    context.Result = new BadRequestObjectResult("idSucursal debe ser un número válido.");
                    return;
                }

                // Llamada asíncrona sincrónicamente
                var isSucursalAsociadaAEmpresa = _sucursalRepository.SucursalAsociadaAEmpresaAsync(nitEmpresaClaim, idSucursal).GetAwaiter().GetResult();

                if (string.IsNullOrEmpty(nitEmpresaClaim) || nitEmpresaClaim != nitEmpresaQuery ||
                    !isSucursalAsociadaAEmpresa)
                {
                    context.Result = new ForbidResult(); // No autorizado
                    return;
                }

            }
        }
    }
}
