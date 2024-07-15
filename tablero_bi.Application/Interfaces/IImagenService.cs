using Microsoft.AspNetCore.Http;
using tablero_bi.Application.Common;
using tablero_bi.Application.DTOs.Empresas;

namespace tablero_bi.Application.Interfaces
{
    public interface IImagenService
    {
        Task<Result<TDto>> SubirImagen<TDto>(IFormFile file, string rutaCarpetas, Func<string, TDto> crearDto);
        Task<Result<TDto>> EditarImagen<TDto>(IFormFile file, string rutaCarpetas, Func<string, TDto> editarDto, string urlImagenAnterior);

        Task<Result<bool>> EliminarImagen(string urlImagenAnterior);

    }
}
