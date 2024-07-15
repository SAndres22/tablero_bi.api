using Microsoft.AspNetCore.Http;
using tablero_bi.Application.Common;

namespace tablero_bi.Application.Interfaces
{
    public interface IUploadImagenService
    {
        Task<Result<TDto>> SubirImagen<TDto>(IFormFile file, string rutaCarpetas, Func<string, TDto> crearDto);


    }
}
