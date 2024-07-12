using tablero_bi.Application.Common;
using tablero_bi.Application.DTOs.Empresas;

namespace tablero_bi.Application.Interfaces
{
    public interface IEmpresaService
    {
        Task<Result<EmpresaDto>> GetEmpresaByNitAsync(string nitEmpresa);
        Task<Result<IEnumerable<EmpresaDto>>> GetEmpresasAsync();
        Task<Result<CreateEmpresaDto>> CreateEmpresaAsync(CreateEmpresaDto empresaDto);
        Task<Result<UpdateEmpresaDto>> EditEmpresaAsync(UpdateEmpresaDto updateEmpresaDto, EmpresaDto empresaDto);
        Task<Result<bool>> DeleteEmpresaAsync (string nitEmpresa);
    }
}
