using FluentValidation;

namespace tablero_bi.Application.DTOs.Login
{
    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string NitEmpresa { get; set; }
    }


    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre de usuario no puede tener más de 50 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MaximumLength(100).WithMessage("La contraseña no puede tener más de 100 caracteres.");

            RuleFor(x => x.NitEmpresa)
                .NotEmpty().WithMessage("El NIT de la empresa es obligatorio.")
                .MaximumLength(20).WithMessage("El NIT de la empresa no puede tener más de 20 caracteres.");
        }
    }

}
