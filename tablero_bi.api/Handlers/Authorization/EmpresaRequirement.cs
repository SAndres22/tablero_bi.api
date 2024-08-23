using Microsoft.AspNetCore.Authorization;

namespace tablero_bi.api.Handlers.Authorization
{
    public class EmpresaRequirement : IAuthorizationRequirement
    {
        public EmpresaRequirement() { }
    }
}
