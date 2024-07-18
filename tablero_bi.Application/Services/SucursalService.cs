using AutoMapper;
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
        private readonly IMapper _mapper;

        public SucursalService(ISucursalRepository sucursalRepository, IEmpresaRepository empresaRepository,
            IMapper mapper)
        {
            _sucursalRepository = sucursalRepository;
            _empresaRepository = empresaRepository;
            _mapper = mapper;
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

        public async Task<Result<IEnumerable<SucursalDto>>> GetSucursalesAsync(string nitEmpresa)
        {
            if (string.IsNullOrWhiteSpace(nitEmpresa))
            { 
                return new Result<IEnumerable<SucursalDto>>().Failed(new List<string> { "El campo no puede estar vacio" });
            }

            var empresaExiste = await _empresaRepository.CompanyExistsByNitAsync(nitEmpresa);
            if (!empresaExiste)
            {
                return new Result<IEnumerable<SucursalDto>>().Failed(new List<string> { "Error la empresa no existe." });
            }

            var listSucursales = await _sucursalRepository.GetSucursalesAsync(nitEmpresa);

            var listSucursalesDto = _mapper.Map<IEnumerable<SucursalDto>>(listSucursales);
            if(listSucursales == null || !listSucursales.Any())
            {
                return new Result<IEnumerable<SucursalDto>>().Success(listSucursalesDto, new List<string> { " No hay sucursales para la empresa" });
            }

            return new Result<IEnumerable<SucursalDto>>().Success(listSucursalesDto, new List<string> { " Sucursales encontradas" });
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
