using tablero_bi.Domain.Entities;

namespace tablero_bi.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuarios> GetLoginUserAsync(Usuarios usuario);
        Task<Usuarios> GetUserByUsername(string username);
    }
}
