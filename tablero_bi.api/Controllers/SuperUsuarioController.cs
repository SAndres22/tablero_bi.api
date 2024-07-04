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
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenValido = await _tokenService.ValidateToken(token);
            if (!tokenValido)
            {
                return BadRequest("Ud no es un SuperUsuario");
            }
            return Ok("This is an SuperUser endpoint");
        }


        [HttpDelete("DeleteTokenExpiredBD")]
        public async Task<IActionResult> DeleteTokensDB()
        {
            var result = await AdminEndpoint();

            if (result is OkObjectResult)
            {
                await _tokenService.CleanExpiredTokens();
                return Ok("Tokens eliminados correctamente");
            }
            else
            {
                return result;
            }
        }


    }
}
