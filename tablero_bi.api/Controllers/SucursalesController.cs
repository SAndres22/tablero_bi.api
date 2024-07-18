using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.api.Attributes;
using tablero_bi.Application.DTOs.Sucursales;
using tablero_bi.Application.Interfaces;
using tablero_bi.Application.Services;

namespace tablero_bi.api.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    [CheckEmpresa]
    public class SucursalesController : ControllerBase
    {
        private readonly ISucursalService _sucursalService;

        public SucursalesController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        [HttpGet("GetSucursales")]
        public async Task<IActionResult> GetSucursales(string nitEmpresa)
        {
            var result = await _sucursalService.GetSucursalesAsync(nitEmpresa);
            return result.IsSuccess 
                ? (IActionResult)Ok(result) 
                : BadRequest(result);
        }

        [HttpGet("GetSucursalById")]
        public async Task<IActionResult> GetSucursalById([FromQuery] string nitEmpresa, int idSucursal)
        {
            var result = await _sucursalService.GetSucursalByIdAsync(idSucursal, nitEmpresa);
            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : BadRequest(result);
        }


        [HttpPost("CreateNewSucursal")]
        public async Task<IActionResult> CreateNewSucursal([FromForm]CreateSucursalDto sucursalDto, [FromQuery] string nitEmpresa)
        {
            var result = await _sucursalService.CreateNewSucursalAsync(sucursalDto);
            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : BadRequest(result);
        }

    }
}
