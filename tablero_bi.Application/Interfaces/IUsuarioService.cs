using tablero_bi.Application.Common;
using tablero_bi.Application.DTOs.Login;

namespace tablero_bi.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest);
    }
}
