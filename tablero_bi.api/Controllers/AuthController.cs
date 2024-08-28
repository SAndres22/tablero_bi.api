using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.Application.DTOs.Login;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;
        private readonly ILoggerService _logger;
        public AuthController(IUsuarioService usuarioService, ITokenService tokenService, ILoggerService logger)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("IniciarSesion")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var result = await _usuarioService.LoginAsync(loginRequest);
            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : BadRequest(result);
        }

        [HttpPost("CerrarSesion")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var result = await _tokenService.RevokeToken(token);
            return result.IsSuccess
                ? (IActionResult)Ok(result.message) : BadRequest(result.message);
        }
    }
}