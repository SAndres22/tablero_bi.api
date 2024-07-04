namespace tablero_bi.Domain.Interfaces
{
    public interface IEmpresaRepository
    {
        Task<bool> CompanyExistsAsync(string nitEmpresa);
        Task<bool> CompanyAssociatedUser(string nitEmpresa, string username);
    }
}
