namespace tablero_bi.Domain.Entities
{
    public class Empresas
    {
        public int EmpresaId { get; set; }
        public string Nit { get; set; }
        public string NombreEmpresa { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public DateTime FechaDeSistema { get; set; }
        public List<Sucursales> Sucursales { get; set; }
    }
}
