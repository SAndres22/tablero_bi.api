namespace tablero_bi.Application.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string NitEmpresa { get; set; }
    }
}
