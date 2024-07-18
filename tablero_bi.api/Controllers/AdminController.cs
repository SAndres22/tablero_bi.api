using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace tablero_bi.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : ControllerBase
    {
        public AdminController()
        {}

        [HttpGet("IsAdmin")]
        public async Task<IActionResult> AdminEndpoint()
        {
            return Ok("This is an Admin endpoint");
        }


    }
}
