using tablero_bi.Domain.Entities;

namespace tablero_bi.Domain.Interfaces
{
    public interface ITokenRepository
    {
        Task<int> AddRevokedToken(RevokedToken token);
        Task<bool> IsTokenRevoked();
        Task CleanExpiredTokens();
        
    }
}
