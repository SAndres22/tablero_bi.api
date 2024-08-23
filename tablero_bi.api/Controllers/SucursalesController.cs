using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.Application.DTOs.Sucursales;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class SucursalesController : ControllerBase
    {
        private readonly ISucursalService _sucursalService;
        private readonly IAuthorizationService _authorizationService;

        public SucursalesController(ISucursalService sucursalService, IAuthorizationService authorizationService)
        {
            _sucursalService = sucursalService;
            _authorizationService = authorizationService;

        }

        [HttpGet("GetAllSucursales")]
        public async Task<IActionResult> GetAllSucursales([FromQuery]string nitEmpresa)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, nitEmpresa, "CheckEmpresaPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _sucursalService.GetSucursalesAsync(nitEmpresa);
            return result.IsSuccess ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpGet("GetSucursalById")]
        public async Task<IActionResult> GetSucursalById(int idSucursal, [FromQuery] string nitEmpresa)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, nitEmpresa, "CheckEmpresaPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _sucursalService.GetSucursalByIdAsync(idSucursal, nitEmpresa);
            return result.IsSuccess ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost("CreateNewSucursal")]
        public async Task<IActionResult> CreateNewSucursal([FromForm]CreateSucursalDto sucursalDto, [FromQuery] string nitEmpresa)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, nitEmpresa, "CheckEmpresaPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _sucursalService.CreateNewSucursalAsync(sucursalDto,nitEmpresa);
            return result.IsSuccess ? (IActionResult)Ok(result) : BadRequest(result);
        }

        //[HttpPut("EditSucursal")]
        //public async Task<IActionResult> EditSucursal([FromForm] UpdateSucursalDto sucursalDto)



    }
}
