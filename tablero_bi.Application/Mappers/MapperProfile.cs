using AutoMapper;
using tablero_bi.Application.DTOs.Empresas;
using tablero_bi.Application.DTOs.Login;
using tablero_bi.Application.DTOs.Sucursales;
using tablero_bi.Domain.Entities;

namespace tablero_bi.Application.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<LoginRequestDto, Usuarios>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForPath(dest => dest.Empresas.Nit, opt => opt.MapFrom(src => src.NitEmpresa))
            .ReverseMap();


            CreateMap<Usuarios, LoginResponseDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore()) // Ignora inicialmente el token
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.NitEmpresa, opt => opt.MapFrom(src => src.Empresas.Nit))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Roles.Name)).ReverseMap();



            CreateMap<Empresas, EmpresaDto>().ReverseMap();
            CreateMap<Sucursales, SucursalDto>().ReverseMap();

        }
    }
}
