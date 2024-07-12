namespace tablero_bi.Domain.Entities
{
    public class Usuarios
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int EmpresaId { get; set; }
        public string ImagenUrl { get; set; }
        public string RoleId { get; set; }

        public Empresas Empresas { get; set; }
        public Roles Roles { get; set; }
    }
}
