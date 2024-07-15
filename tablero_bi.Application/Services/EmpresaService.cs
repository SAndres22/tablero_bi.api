using AutoMapper;
using tablero_bi.Application.Common;
using tablero_bi.Application.DTOs.Empresas;
using tablero_bi.Application.Interfaces;
using tablero_bi.Domain.Entities;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.Application.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IImagenService _imagenService;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IMapper _mapper;

        public EmpresaService(IEmpresaRepository empresaRepository, IMapper mapper, 
            IImagenService imagenService)
        {
            _empresaRepository = empresaRepository;
            _mapper = mapper;
            _imagenService = imagenService;
        }

        public async Task<Result<CreateEmpresaDto>> CreateEmpresaAsync(CreateEmpresaDto empresaDto)
        {
            var validationResult = ValidateCreateEmpresaRequest(empresaDto);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var companyExists = await _empresaRepository.CompanyExistsByNitAsync(empresaDto.Nit);
            if (companyExists)
            {
                return new Result<CreateEmpresaDto>().Failed(new List<string> { "El NIT de la empresa ya existe." });
            }

            var empresaImagen = await _imagenService.SubirImagen(empresaDto.LogoFile, "Images/Logos", (url) => new CreateEmpresaDto { Logo = url });

            if (!empresaImagen.IsSuccess)
            {
                return empresaImagen;
            }

            var nuevaEmpresa = new Empresas()
            {
                Nit = empresaDto.Nit,
                NombreEmpresa = empresaDto.NombreEmpresa,
                Logo = empresaImagen.data.Logo,
                Email = empresaDto.Email,
            };

            var empresaCreada = await _empresaRepository.CreateEmpresaAsync(nuevaEmpresa);
            if (!empresaCreada)
            {
                return new Result<CreateEmpresaDto>().Failed(new List<string> { "Error al crear la empresa." });
            }

            return new Result<CreateEmpresaDto>().Success(null,new List<string> { "Empresa Creada exitosamente."});
        }

        public async Task<Result<bool>> DeleteEmpresaAsync(string nitEmpresa)
        {
            if (string.IsNullOrWhiteSpace(nitEmpresa))
            {
                return new Result<bool>().Failed(new List<string> { "El Nit no puede estar vacio" });
            }

            var empresaActual = await _empresaRepository.GetEmpresaByNitAsync(nitEmpresa);

            if (empresaActual == null)
            {
                return new Result<bool>().Failed(new List<string> { "No existe la empresa" });
            }

            var isDelete = await _empresaRepository.DeleteEmpresaAsync(nitEmpresa);

            if (!isDelete)
            {
                return new Result<bool>().Failed(new List<string> { "No se logro eliminar la empresa" });
            }

            await _imagenService.EliminarImagen(empresaActual.Logo);

            return new Result<bool>().Success(true, new List<string> { "Empresa Eliminada" });

        }

        public async Task<Result<UpdateEmpresaDto>> EditEmpresaAsync(UpdateEmpresaDto updateEmpresaDto)
        {
            var empresaActual = await _empresaRepository.GetEmpresaByNitAsync(updateEmpresaDto.Nit);

            if (empresaActual == null)
            {
                return new Result<UpdateEmpresaDto>().Failed(new List<string> { "No existe la empresa" });
            }

            var empresaImagen = await _imagenService.EditarImagen(updateEmpresaDto.LogoFile, "Images/Logos", (url) => new UpdateEmpresaDto { Logo = url }, empresaActual.Logo);

            if (!empresaImagen.IsSuccess)
            {
                return empresaImagen;
            }

            var empresa = new Empresas()
            {
                NombreEmpresa = updateEmpresaDto.NombreEmpresa,
                Email = updateEmpresaDto.Email,
                Logo = empresaImagen.data.Logo
                
            };

            var isUpdated = await _empresaRepository.EditEmpresaAsync(empresa, updateEmpresaDto.Nit);

            if (!isUpdated)
            {
                return new Result<UpdateEmpresaDto>().Failed(new List<string> { "No se realizaron cambios." });
            }

            return new Result<UpdateEmpresaDto>().Success(null, new List<string> { "Empresa actualizada exitosamente." });

        }

        public async Task<Result<EmpresaDto>> GetEmpresaByNitAsync(string nitEmpresa)
        {
            if (string.IsNullOrWhiteSpace(nitEmpresa))
            {
                return new Result<EmpresaDto>().Failed(new List<string> { "El Nit no puede estar vacio" });
            }

            var empresaEncontrada = await _empresaRepository.GetEmpresaByNitAsync(nitEmpresa);
            if(empresaEncontrada == null)
            {
                return new Result<EmpresaDto>().Failed(new List<string> { "No se encontro la empresa" });
            }

            var empresa = _mapper.Map<EmpresaDto>(empresaEncontrada);

            return new Result<EmpresaDto>().Success(empresa, new List<string> { "Empresa encontrada" });
        }

        public async Task<Result<IEnumerable<EmpresaDto>>> GetEmpresasAsync()
        {
            var listEmpresas = await _empresaRepository.GetAllEmpresasAsync();
            if(listEmpresas == null || !listEmpresas.Any())
            {
                return new Result<IEnumerable<EmpresaDto>>().Failed(new List<string> { "No hay empresas registradas" });
            }

            var listEmpresasDto = _mapper.Map<IEnumerable<EmpresaDto>>(listEmpresas);

            return new Result<IEnumerable<EmpresaDto>>().Success(listEmpresasDto, new List<string> { "Listado de Empresas encontrado" });
        }

        private Result<CreateEmpresaDto> ValidateCreateEmpresaRequest(CreateEmpresaDto empresaDto)
        {
            if (string.IsNullOrWhiteSpace(empresaDto.NombreEmpresa) ||
                string.IsNullOrWhiteSpace(empresaDto.Nit))
            {
                return new Result<CreateEmpresaDto>().Failed(new List<string> { "Los campos no pueden estar vacios" });
            }

            return new Result<CreateEmpresaDto>().Success(null);
        }
    }
}
