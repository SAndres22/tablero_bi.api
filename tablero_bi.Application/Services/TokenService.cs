using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tablero_bi.Application.Common;
using tablero_bi.Application.Interfaces;
using tablero_bi.Domain.Entities;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly ITokenRepository _tokenRepository;

        public TokenService(IConfiguration configuration, IUsuarioRepository usuarioRepository,
            ITokenRepository tokenRepository)
        {
            _config = configuration;
            _tokenRepository = tokenRepository;

        }

        public string GenerateToken(string username, string role, string nitEmpresa)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var now = DateTime.UtcNow.ToLocalTime();
            var expiration = now.AddMinutes(120);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, _config["Jwt:Issuer"]),
                new Claim("nitEmpresa", nitEmpresa),
                new Claim(ClaimTypes.Role, role),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore :now,
                expires: expiration,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Result<RevokedToken>> RevokeToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new Result<RevokedToken>().Failed(new List<string> { "Token no valido" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo.ToLocalTime();

            var revokedToken = new RevokedToken
            {
                Token = token,
                RevokedAt = DateTime.UtcNow.ToLocalTime(),
                Expiration = expiration
            };

            var tokenAgregadoBD = await _tokenRepository.AddRevokedToken(revokedToken);
            if(tokenAgregadoBD == 0)
            {
                return new Result<RevokedToken>().Failed(new List<string> { "Error de Token" });
            }

            return new Result<RevokedToken>().Success(revokedToken, new List<string> {  "Sesion cerrada exitosamente" });

        }

        public async Task<bool> IsTokenRevoked(string token)
        {
            return await _tokenRepository.IsTokenRevoked(token);
        }

        public async Task CleanExpiredTokens()
        {
            await _tokenRepository.CleanExpiredTokens();
        }
    }
}
