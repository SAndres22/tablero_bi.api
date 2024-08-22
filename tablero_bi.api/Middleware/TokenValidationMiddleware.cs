using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.api.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<TokenValidationMiddleware> _logger;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration config, IServiceScopeFactory serviceScopeFactory, 
            ILogger<TokenValidationMiddleware> logger)
        {
            _next = next;
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null && await ValidateToken(token))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token validation failed.");
            }
        }

        private async Task<bool> ValidateToken(string token)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
                var tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                };

                try
                {
                    var tokenRevocado = await tokenRepository.IsTokenRevoked(token);
                     
                    if(tokenRevocado == true)
                    {
                        return false;
                    }

                    SecurityToken validatedToken;
                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

                    // Extraer reclamaciones del token validado
                    var jwtToken = validatedToken as JwtSecurityToken;
                    var username = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;
                    var role = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                    var nitEmpresa = jwtToken.Claims.FirstOrDefault(x => x.Type == "nitEmpresa")?.Value;

                    var userFromDatabase = await userRepository.GetUserByUsername(username);

                    if (userFromDatabase != null &&
                        userFromDatabase.Roles.Name == role &&
                        userFromDatabase.Empresas.Nit == nitEmpresa)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Token validation failed.");
                }

                return false;
            }
        }
    }
}
