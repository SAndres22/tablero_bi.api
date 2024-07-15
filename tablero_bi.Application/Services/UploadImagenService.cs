using Aspose.Imaging;
using Aspose.Imaging.FileFormats.Png;
using Microsoft.AspNetCore.Http;
using tablero_bi.Application.Common;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.Application.Services
{
    public class UploadImagenService : IUploadImagenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadImagenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<TDto>> SubirImagen<TDto>(IFormFile file, string rutaCarpetas, Func<string, TDto> crearDto)
        {
            string pathImages = ObtenerRutaDeAlmacenamiento(rutaCarpetas);

            if (!Directory.Exists(pathImages))
            {
                Directory.CreateDirectory(pathImages);
            }

            if (file == null || file.Length == 0)
            {
                var dto = crearDto(null);
                return new Result<TDto>().Success(dto, new List<string> { null });
            }

            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!EsExtensionValida(extension))
            {
                return new Result<TDto>().Failed(new List<string> { "La extensión del archivo no es válida. Solo se permiten archivos con extensiones .svg o .png " });
            }

            string newNombreImagen = GenerarNombreImagen(file.FileName, extension);
            string rutaCompleta = Path.Combine(pathImages, newNombreImagen);

            try
            {
                // Guardar temporalmente la imagen en el servidor
                string tempPath = Path.Combine(pathImages, $"temp_{newNombreImagen}");

                using (var fileStream = new FileStream(tempPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Si la imagen es SVG, mover el archivo directamente sin procesamiento
                if (extension == ".svg")
                {
                    File.Move(tempPath, rutaCompleta);
                }
                else
                {
                    // Usando la Libreria Aspose para procesar la imagen PNG
                    using (var original = Image.Load(tempPath))
                    {
                        if (original is PngImage pngImage)
                        {
                            string rutaSvg = Path.ChangeExtension(rutaCompleta, ".svg");
                            ImageHelper.convertirPngASvg(pngImage, rutaSvg);
                            newNombreImagen = Path.GetFileName(rutaSvg);
                        }
                    }

                    // Eliminar la imagen temporal
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                }

                var request = _httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host.Value}";

                var dto = crearDto($"{baseUrl}/{rutaCarpetas}/{newNombreImagen}");
                return new Result<TDto>().Success(dto, new List<string> { null });
            }
            catch (IOException ioEx)
            {
                return new Result<TDto>().Failed(new List<string> { "Error al guardar el archivo en el servidor: " + ioEx.Message });

            }
        }


        private string ObtenerRutaDeAlmacenamiento(string rutaCarpetas)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), rutaCarpetas);
        }

        private string GenerarNombreImagen(string fileName, string extension)
        {
            string nombreImagen = Path.GetFileNameWithoutExtension(fileName);
            return $"{nombreImagen}{Guid.NewGuid()}{extension}";
        }

        private bool EsExtensionValida(string extension)
        {
            string[] extensionesValidas = { ".svg", ".png" };
            return extensionesValidas.Contains(extension);
        }
    }
}
