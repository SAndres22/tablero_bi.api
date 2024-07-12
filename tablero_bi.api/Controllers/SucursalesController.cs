using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.Application.DTOs;
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
