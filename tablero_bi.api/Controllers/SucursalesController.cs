using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.api.Attributes;
using tablero_bi.Application.DTOs.Sucursales;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.api.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SucursalesController : ControllerBase
    {
        private readonly ISucursalService _sucursalService;

        public SucursalesController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [CheckEmpresa]
        [HttpGet("GetSucursales")]
        public async Task<IActionResult> GetSucursales(string nitEmpresa)
        {
            var result = await _sucursalService.GetSucursalesAsync(nitEmpresa);
            return result.IsSuccess 
                ? (IActionResult)Ok(result) 
                : BadRequest(result);
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("CreateNewSucursal")]
        public async Task<IActionResult> CreateNewSucursal(CreateSucursalDto sucursalDto)
        {
            var result = await _sucursalService.CreateNewSucursalAsync(sucursalDto);
            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : BadRequest(result);
        }

    }
}
