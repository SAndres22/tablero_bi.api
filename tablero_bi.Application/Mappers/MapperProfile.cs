using AutoMapper;
using tablero_bi.Application.DTOs.Empresas;
using tablero_bi.Application.DTOs.Login;
using tablero_bi.Domain.Entities;

namespace tablero_bi.Application.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<LoginRequestDto, Usuarios>().ReverseMap();
            CreateMap<Usuarios, LoginResponseDto>().ReverseMap();
            
            CreateMap<Empresas, EmpresaDto>().ReverseMap();

        }
    }
}
