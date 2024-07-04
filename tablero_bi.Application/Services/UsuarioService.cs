using AutoMapper;
using tablero_bi.Application.Common;
using tablero_bi.Application.DTOs;
using tablero_bi.Application.Interfaces;
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

            var companyExists = await _empresaRepository.CompanyExistsAsync(loginRequest.NitEmpresa);
            if (!companyExists)
            {
                return new Result<LoginResponseDto>().Failed(new List<string> { "El NIT de la empresa no existe." });
            }

            var companyAssociatedUser = await _empresaRepository.CompanyAssociatedUser(loginRequest.NitEmpresa, loginRequest.Username);
            if (!companyAssociatedUser)
            {
                return new Result<LoginResponseDto>().Failed(new List<string> { "El NIT no esta asociado a un Usuario" });
            }

            var passwordHash = _cifradoService.Encriptar(loginRequest.Password);

            var usuarioFromRepo = await _usuarioRepository.GetLoginUserAsync(loginRequest.NitEmpresa, loginRequest.Username, passwordHash);
            if (usuarioFromRepo == null)
            {
                return new Result<LoginResponseDto>().Failed(new List<string> { "Usuario o contraseña incorrectos" });
            }

            var loginResponseDto = new LoginResponseDto
            {
                Token = _tokenService.GenerateToken(usuarioFromRepo.Username, usuarioFromRepo.Roles.Name),
                Username = usuarioFromRepo.Username,
                NitEmpresa = usuarioFromRepo.Empresas.Nit,
                Role = usuarioFromRepo.Roles.Name,
            };

            var validar = await _tokenService.ValidateToken(loginResponseDto.Token);
            if(validar == false)
            {
                return new Result<LoginResponseDto>().Failed(new List<string> { "EL token no es seguro" });
            }

            var usuarioDto = _mapper.Map<LoginResponseDto>(loginResponseDto);
            return new Result<LoginResponseDto>().Success(usuarioDto, new List<string> { "Sesion iniciada exitosamente"});
        }

        private Result<LoginResponseDto> ValidateLoginRequest(LoginRequestDto loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Username) ||
                string.IsNullOrWhiteSpace(loginRequest.Password) ||
                string.IsNullOrWhiteSpace(loginRequest.NitEmpresa))
            {
                return new Result<LoginResponseDto>().Failed(new List<string> { "Los campos no pueden estar vacios" });
            }

            return new Result<LoginResponseDto>().Success(null);
        }
    }
}
