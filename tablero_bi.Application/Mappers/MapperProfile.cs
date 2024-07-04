using AutoMapper;
using tablero_bi.Application.DTOs;
using tablero_bi.Domain.Entities;

namespace tablero_bi.Application.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<LoginRequestDto, Usuarios>().ReverseMap();
            CreateMap<Usuarios, LoginResponseDto>().ReverseMap();
            
        }
    }
}
