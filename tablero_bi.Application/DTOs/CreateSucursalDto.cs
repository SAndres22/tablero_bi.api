using System.ComponentModel.DataAnnotations;

namespace tablero_bi.Application.DTOs
{
    public class CreateSucursalDto
    {
        [Required(ErrorMessage ="El nombre de la Sucursal es Requerido")]
        public string NombreSucursal { get; set; }
        [Required(ErrorMessage = "El id de la empresa es Requerido")]
        public int EmpresaId { get; set; }
    }
} 
