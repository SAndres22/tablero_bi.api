
using tablero_bi.Application.DTOs.Sucursales;

namespace tablero_bi.Application.DTOs.Empresas
{
    public class EmpresaDto
    {
        public int EmpresaId { get; set; }
        public string Nit { get; set; }
        public string NombreEmpresa { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public DateTime FechaDeSistema { get; set; }
        public List<SucursalDto> Sucursales { get; set; }
    }
}
