using tablero_bi.Domain.Entities;

namespace tablero_bi.Domain.Interfaces
{
    public interface ISucursalRepository
    {
        Task<Sucursales> GetSucursalByIdAsync(int id);
        Task<bool> CreateNewSucursalAsync(Sucursales sucursal);
        Task<IEnumerable<Sucursales>> GetSucursalesAsync(string nitEmpresa);
        Task<bool> GetSucursalAsociadaToEmpresaAsync(int empresaId, string nitEmpresa);
    }
}
