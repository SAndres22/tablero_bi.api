using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.api.Attributes;
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

        public SucursalesController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        [HttpGet("GetAllSucursales")]
        [CheckEmpresa]
        public async Task<IActionResult> GetAllSucursales([FromQuery]string nitEmpresa)
        {
            var result = await _sucursalService.GetSucursalesAsync(nitEmpresa);
            return result.IsSuccess 
                ? (IActionResult)Ok(result) 
                : BadRequest(result);
        }

        [HttpGet("GetSucursalById")]
        [CheckEmpresa]
        public async Task<IActionResult> GetSucursalById(int idSucursal, [FromQuery] string nitEmpresa)
        {
            var result = await _sucursalService.GetSucursalByIdAsync(idSucursal, nitEmpresa);
            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : BadRequest(result);
        }

        [HttpPost("CreateNewSucursal")]
        [CheckEmpresa]
        public async Task<IActionResult> CreateNewSucursal([FromForm]CreateSucursalDto sucursalDto, [FromQuery] string nitEmpresa)
        {
            var result = await _sucursalService.CreateNewSucursalAsync(sucursalDto,nitEmpresa);
            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : BadRequest(result);
        }

        //[HttpPut("EditSucursal")]
        //public async Task<IActionResult> EditSucursal([FromForm] UpdateSucursalDto sucursalDto)



    }
}
