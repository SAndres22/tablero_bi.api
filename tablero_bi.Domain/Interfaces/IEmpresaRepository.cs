using tablero_bi.Domain.Entities;

namespace tablero_bi.Domain.Interfaces
{
    public interface IEmpresaRepository
    {
        Task<IEnumerable<Empresas>> GetAllEmpresasAsync();
        Task<IEnumerable<Empresas>> GetAllEmpresasAndSucursales();
        Task<Empresas> GetEmpresaByNitAsync(string nit);
        Task<bool> CompanyExistsByNitAsync(string nitEmpresa);
        Task<bool> CompanyExistsByIdAsync(int id);
        Task<bool> CompanyAssociatedUser(string nitEmpresa, string username);
        Task<bool> CreateEmpresaAsync(Empresas empresa);
        Task<bool> EditEmpresaAsync(Empresas empresas, string nit);
        Task<bool> DeleteEmpresaAsync(string nitEmpresa);
    }
}
