using tablero_bi.Application.Common;
using tablero_bi.Domain.Entities;

namespace tablero_bi.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string username, string role);
        Task<bool> ValidateToken(string token);
        Task<Result<RevokedToken>> RevokeToken(string token);
        Task<bool> IsTokenRevoked(string token);
        Task CleanExpiredTokens();

    }
}
