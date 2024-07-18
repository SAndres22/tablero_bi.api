using tablero_bi.Domain.Entities;

namespace tablero_bi.Domain.Interfaces
{
    public interface ISucursalRepository
    {
        Task<bool> CreateNewSucursalAsync(Sucursales sucursal);
        Task<IEnumerable<Sucursales>> GetSucursalesAsync(string nitEmpresa);
    }
}
