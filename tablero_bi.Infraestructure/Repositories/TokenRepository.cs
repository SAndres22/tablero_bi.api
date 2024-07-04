using Dapper;
using System.Data;
using tablero_bi.Domain.Entities;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.Infraestructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IDbConnection _db;

        public TokenRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<bool> IsTokenRevoked(string token)
        {
            var result = await _db.QueryFirstOrDefaultAsync<RevokedToken>("SELECT * FROM RevokedTokens WHERE Token = @Token", new { Token = token });
            return result != null;
        }

        public async Task<int> AddRevokedToken(RevokedToken token)
        {
            var result = await _db.ExecuteAsync("INSERT INTO RevokedTokens (Token, RevokedAt, Expiration) VALUES (@Token, @RevokedAt, @Expiration)", token);
            return result;
        }

        public async Task CleanExpiredTokens()
        {
            var now = DateTime.UtcNow.ToLocalTime();
            await _db.ExecuteAsync("DELETE FROM RevokedTokens WHERE Expiration < @Now", new { Now = now });
        }
    }


}
