using System.ComponentModel.DataAnnotations;

namespace tablero_bi.Application.DTOs.Empresas
{
    public class UpdateEmpresaDto
    {
        [Required(ErrorMessage = "El Nit de la empresa es Obligatorio")]
        public string Nit { get; set; }

        [Required(ErrorMessage = "El Nombre de la empresa es Obligatorio")]
        public string NombreEmpresa { get; set; }
    }
}
