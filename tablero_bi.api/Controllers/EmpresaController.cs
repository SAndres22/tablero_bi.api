using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.Application.DTOs.Empresas;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;
        private readonly IAuthorizationService _authorizationService;

        public EmpresaController(IEmpresaService empresaService, IAuthorizationService authorizationService)
        {
            _empresaService = empresaService;
            _authorizationService = authorizationService;
        }

        [HttpGet("GetEmpresas")]
        [Authorize(Policy = "RequireSuperUserRole")]
        public async Task<IActionResult> GetEmpresas()
        {
            var result = await _empresaService.GetEmpresasAsync();
            return result.IsSuccess ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpGet("GetEmpresasConSucursales")]
        [Authorize(Policy = "RequireSuperUserRole")]
        public async Task<IActionResult> GetEmpresasAllSucursales()
        {
            var result = await _empresaService.GetEmpresasAllSucursalesAsync();
            return result.IsSuccess ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpGet("GetEmpresaByNit")]
        [Authorize(Policy = "RequireUserRole")] // Mantén la política de autorización existente
        public async Task<IActionResult> GetEmpresaByNit(string nitEmpresa)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, nitEmpresa, "CheckEmpresaPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid(); 
            }

            var result = await _empresaService.GetEmpresaByNitAsync(nitEmpresa);
            return result.IsSuccess ? (IActionResult)Ok(result) : BadRequest(result);
        }


        [HttpPost("CreateEmpresa")]
        [Authorize(Policy = "RequireSuperUserRole")]
        public async Task<IActionResult> CreateEmpresa([FromForm] CreateEmpresaDto empresaDto)
        {
            var result = await _empresaService.CreateEmpresaAsync(empresaDto);
            return result.IsSuccess ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPut("EditEmpresa")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> EditEmpresa([FromForm]UpdateEmpresaDto empresaDto, [FromQuery] string nitEmpresa)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, nitEmpresa, "CheckEmpresaPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            var result = await _empresaService.EditEmpresaAsync(empresaDto);
            return result.IsSuccess ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpDelete("DeleteEmpresa")]
        [Authorize(Policy = "RequireSuperUserRole")]
        public async Task<IActionResult> DeleteEmpresa([FromQuery] string nitEmpresa)
        {
            var result = await _empresaService.DeleteEmpresaAsync(nitEmpresa);
            return result.IsSuccess ? (IActionResult)Ok(result) : BadRequest(result);
        }
    }
}
