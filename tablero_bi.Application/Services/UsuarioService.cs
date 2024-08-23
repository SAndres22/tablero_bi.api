using AutoMapper;
using tablero_bi.Application.Common;
using tablero_bi.Application.DTOs.Login;
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
            var validator = new LoginRequestValidator();
            var validationResult = validator.Validate(loginRequest);

            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return new Result<LoginResponseDto>().Failed(validationErrors);
            }

            var companyExists = await _empresaRepository.CompanyExistsByNitAsync(loginRequest.NitEmpresa);
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
                Token = _tokenService.GenerateToken(usuarioFromRepo.Username, usuarioFromRepo.Roles.Name, loginRequest.NitEmpresa),
                Username = usuarioFromRepo.Username,
                NitEmpresa = usuarioFromRepo.Empresas.Nit,
                Role = usuarioFromRepo.Roles.Name,
            };

            var usuarioDto = _mapper.Map<LoginResponseDto>(loginResponseDto);
            return new Result<LoginResponseDto>().Success(usuarioDto, new List<string> { "Sesion iniciada exitosamente"});
        }

    }
}
