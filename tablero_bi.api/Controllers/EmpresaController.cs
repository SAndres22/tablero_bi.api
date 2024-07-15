﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tablero_bi.Application.DTOs.Empresas;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmpresaController : ControllerBase
    {

        private readonly IEmpresaService _empresaService;

        public EmpresaController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }


        [HttpGet("GetEmpresas")]
        [Authorize(Policy = "RequireSuperUserRole")]
        public async Task<IActionResult> GetEmpresas()
        {
            var result = await _empresaService.GetEmpresasAsync();
            return result.IsSuccess
                ?(IActionResult)Ok(result)
                : BadRequest();
        }


        [HttpGet("GetEmpresaByNit")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<IActionResult> GetEmpresaByNit(string nitEmpresa)
        {
            var result = await _empresaService.GetEmpresaByNitAsync(nitEmpresa);
            return result.IsSuccess
                ?(IActionResult)Ok(result) 
                : BadRequest(result);
        }

        [HttpPost("CreateEmpresa")]
        [Authorize(Policy = "RequireSuperUserRole")]
        public async Task<IActionResult> CreateEmpresa([FromForm] CreateEmpresaDto empresaDto)
        {
            var result = await _empresaService.CreateEmpresaAsync(empresaDto);
            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : BadRequest(result);
        }

        [HttpPut("EditEmpresa")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> EditEmpresa(UpdateEmpresaDto empresaDto)
        {
            var empresaActualResult = await _empresaService.GetEmpresaByNitAsync(empresaDto.Nit);

            if (!empresaActualResult.IsSuccess)
            {
                return BadRequest(empresaActualResult);
            }

            var empresaActual = empresaActualResult.data;
            var result = await _empresaService.EditEmpresaAsync(empresaDto, empresaActual);

            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : BadRequest(result);
        }

        [HttpDelete("DeleteEmpresa")]
        [Authorize(Policy = "RequireSuperUserRole")]
        public async Task<IActionResult> DeleteEmpresa(string nitEmpresa)
        {
            var empresaToDelete = await _empresaService.GetEmpresaByNitAsync(nitEmpresa);
            if (!empresaToDelete.IsSuccess)
            {
                BadRequest(empresaToDelete);
            }

            var result = await _empresaService.DeleteEmpresaAsync(nitEmpresa);
            return result.IsSuccess
                ?(IActionResult)Ok(result)
                :BadRequest(result);
        }


    }
}
