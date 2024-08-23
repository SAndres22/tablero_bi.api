using AutoMapper;
using tablero_bi.Application.Common;
using tablero_bi.Application.DTOs.Login;
using tablero_bi.Application.Interfaces;
using tablero_bi.Domain.Entities;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly ITokenService _tokenService;
        private readonly ICifradoService _cifradoService;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, 
            IEmpresaRepository empresaRepository, ITokenService tokenService, 
            ICifradoService cifradoService, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
            _tokenService = tokenService;
            _cifradoService = cifradoService;
            _mapper = mapper;

        }

        public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest)
        {

            var validationResult = ValidateLoginRequest(loginRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var empresaResult = await VerificarEmpresaYUsuario(loginRequest.NitEmpresa, loginRequest.Username);
            if (!empresaResult.IsSuccess)
            {
                return empresaResult;
            }

            var usuarioResult = await VerificarCredencialesUsuario(loginRequest);
            if (!usuarioResult.IsSuccess)
            {
                return usuarioResult;
            }

            return usuarioResult;
        }

        private Result<LoginResponseDto> ValidateLoginRequest(LoginRequestDto loginRequest)
        {
            var validator = new LoginRequestValidator();
            var validationResult = validator.Validate(loginRequest);

            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return new Result<LoginResponseDto>().Failed(validationErrors);
            }

            return new Result<LoginResponseDto>().Success(null);
        }

        private async Task<Result<LoginResponseDto>> VerificarEmpresaYUsuario(string nitEmpresa, string username)
        {
            if (!await _empresaRepository.CompanyExistsByNitAsync(nitEmpresa))
            {
                return new Result<LoginResponseDto>().Failed(new List<string> { "El NIT de la empresa no existe." });
            }

            if (!await _empresaRepository.CompanyAssociatedUser(nitEmpresa, username))
            {
                return new Result<LoginResponseDto>().Failed(new List<string> { "El NIT no está asociado a un usuario." });
            }

            return new Result<LoginResponseDto>().Success(null);
        }

        private async Task<Result<LoginResponseDto>> VerificarCredencialesUsuario(LoginRequestDto loginRequest)
        {

            loginRequest.Password = _cifradoService.Encriptar(loginRequest.Password);

            var usuario = _mapper.Map<Usuarios>(loginRequest);

            var usuarioFromRepo = await _usuarioRepository.GetLoginUserAsync(usuario);
            if (usuarioFromRepo == null)
            {
                return new Result<LoginResponseDto>().Failed(new List<string> { "Usuario o contraseña incorrectos" });
            }

            var loginResponseDto = CrearLoginResponseDto(usuarioFromRepo);
            return new Result<LoginResponseDto>().Success(loginResponseDto, new List<string> { "Sesión iniciada exitosamente" });

        }

        private LoginResponseDto CrearLoginResponseDto(Usuarios usuario)
        {
            var token = _tokenService.GenerateToken(usuario.Username, usuario.Roles.Name, usuario.Empresas.Nit);

            var loginResponseDto = _mapper.Map<LoginResponseDto>(usuario, opt =>
            {
                opt.Items["Token"] = token; 
            });

            loginResponseDto.Token = token;

            return loginResponseDto;
        }
    }
}
