namespace tablero_bi.Domain.Entities
{
    public class Sucursales
    {
        public string NombreSucursal { get; set; }
        public int EmpresaId { get; set; }

        public Empresas empresas { get; set; }
    }
}
