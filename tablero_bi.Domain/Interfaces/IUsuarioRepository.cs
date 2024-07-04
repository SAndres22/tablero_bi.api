using tablero_bi.Domain.Entities;

namespace tablero_bi.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuarios> GetLoginUserAsync(string nitEmpresa, string username, string passwordHash);
        Task<Usuarios> GetUserByUsername(string username);
    }
}
