using System.ComponentModel.DataAnnotations;

namespace tablero_bi.Application.DTOs.Login
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, ErrorMessage = "La contraseña no puede tener más de 100 caracteres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El NIT de la empresa es obligatorio.")]
        [StringLength(20, ErrorMessage = "El NIT de la empresa no puede tener más de 20 caracteres.")]
        public string NitEmpresa { get; set; }
    }
}
