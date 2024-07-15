using tablero_bi.Application.Common;
using tablero_bi.Application.DTOs.Sucursales;
using tablero_bi.Application.Interfaces;
using tablero_bi.Domain.Entities;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.Application.Services
{
    public class SucursalService : ISucursalService
    {

        private readonly ISucursalRepository _sucursalRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public SucursalService(ISucursalRepository sucursalRepository, IEmpresaRepository empresaRepository)
        {
            _sucursalRepository = sucursalRepository;
            _empresaRepository = empresaRepository;
        }

        public async Task<Result<CreateSucursalDto>> CreateNewSucursalAsync(CreateSucursalDto sucursalDto)
        {
            var validationResult = ValidateCreateSucursalRequest(sucursalDto);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var empresaExiste = await _empresaRepository.CompanyExistsByIdAsync(sucursalDto.EmpresaId);
            if (!empresaExiste)
            {
                return new Result<CreateSucursalDto>().Failed(new List<string> { "Error la empresa no existe." });
            }

            var newSucursal = new Sucursales(){
                NombreSucursal = sucursalDto.NombreSucursal,
                EmpresaId = sucursalDto.EmpresaId,
            };

            var sucursalCreada = await _sucursalRepository.CreateNewSucursalAsync(newSucursal);
            if (!sucursalCreada)
            {
                return new Result<CreateSucursalDto>().Failed(new List<string> { "Error al crear la Sucursal." });
            }

            return new Result<CreateSucursalDto>().Success(null, new List<string> { "Sucursal Creada exitosamente." });

        }


        private Result<CreateSucursalDto> ValidateCreateSucursalRequest(CreateSucursalDto sucursalDto)
        {
            if (sucursalDto.EmpresaId <= 0 ||
                string.IsNullOrWhiteSpace(sucursalDto.NombreSucursal))
            {
                return new Result<CreateSucursalDto>().Failed(new List<string> { "Los campos no pueden estar vacios" });
            }

            return new Result<CreateSucursalDto>().Success(null);
        }

    }
}
