using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireSuperUserRole")]
    public class SuperUsuarioController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public SuperUsuarioController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("IsSuperUser")]
        public async Task<IActionResult> AdminEndpoint()
        {
            return Ok("This is an SuperUser endpoint");
        }


        [HttpDelete("DeleteTokenExpiredBD")]
        public async Task<IActionResult> DeleteTokensDB()
        {
            await _tokenService.CleanExpiredTokens();
            return Ok("Tokens eliminados correctamente");
        }
    }
}
