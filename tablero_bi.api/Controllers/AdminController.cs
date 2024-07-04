using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AdminController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("IsAdmin")]
        public async Task<IActionResult> AdminEndpoint()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenValido = await _tokenService.ValidateToken(token);
            if (!tokenValido)
            {
                return BadRequest("Ud no es Admin");
            }
            return Ok("This is an Admin endpoint");
        }


    }
}
