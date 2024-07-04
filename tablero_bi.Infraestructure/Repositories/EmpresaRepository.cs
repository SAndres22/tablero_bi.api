using Dapper;
using System.Data;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.Infraestructure.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly IDbConnection _db;

        public EmpresaRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<bool> CompanyAssociatedUser(string nitEmpresa, string username)
        {
            var query = @"SELECT COUNT(1)
                FROM Usuarios u
                INNER JOIN Empresas e ON u.EmpresaId = e.EmpresaId
                WHERE u.Username = @Username
                AND e.Nit = @Nit";

            var count = await _db.ExecuteScalarAsync<int>(query, new {Nit = nitEmpresa, Username = username});
            return count > 0;
        }

        public async Task<bool> CompanyExistsAsync(string nitEmpresa)
        {
            var query = "SELECT COUNT(1) FROM Empresas WHERE Nit = @Nit ";
            var count = await _db.ExecuteScalarAsync<int>(query, new { Nit = nitEmpresa });
            return count > 0;
        }
    }
}
