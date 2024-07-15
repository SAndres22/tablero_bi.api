using Dapper;
using System.Data;
using System.Data.SqlClient;
using tablero_bi.Domain.Entities;
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

            var count = await _db.ExecuteScalarAsync<int>(query, new { Nit = nitEmpresa, Username = username });
            return count > 0;
        }

        public async Task<bool> CompanyExistsByIdAsync(int id)
        {
            var query = "SELECT COUNT(1) FROM Empresas WHERE EmpresaId = @Id ";
            var count = await _db.ExecuteScalarAsync<int>(query, new { Id = id });
            return count > 0;
        }

        public async Task<bool> CompanyExistsByNitAsync(string nitEmpresa)
        {
            var query = "SELECT COUNT(1) FROM Empresas WHERE Nit = @Nit ";
            var count = await _db.ExecuteScalarAsync<int>(query, new { Nit = nitEmpresa });
            return count > 0;
        }

        public async Task<bool> CreateEmpresaAsync(Empresas empresa)
        {
            var query = @"INSERT INTO Empresas(Nit, NombreEmpresa,Logo, Email) VALUES (@Nit, @NombreEmpresa, @Logo, @Email)";
            var count = await _db.ExecuteAsync(query, empresa);

            return count > 0;
        }

        public async Task<bool> DeleteEmpresaAsync(string nitEmpresa)
        {
            var query = @"DELETE FROM Empresas WHERE Nit = @Nit";
            try
            {
                var affectedRows = await _db.ExecuteAsync(query, new { Nit = nitEmpresa });
                return affectedRows > 0;
            }
            catch (SqlException ex) when (ex.Number == 547) // 547 es el error de violación de clave externa
            {
                return false; 
            }
        }

        public async Task<bool> EditEmpresaAsync(Empresas empresas, string nit)
        {
            var query = @"
                UPDATE Empresas
                SET NombreEmpresa = @NombreEmpresa,
                Logo = @Logo, Email = @Email
                WHERE Nit = @Nit"
            ;

            var parameters = new { 
                NombreEmpresa = empresas.NombreEmpresa, 
                Logo = empresas.Logo,
                Email = empresas.Email,
                Nit = nit };

            var affectedRows = await _db.ExecuteAsync(query, parameters);
            return affectedRows > 0;

        }

        public async Task<IEnumerable<Empresas>> GetAllEmpresasAsync()
        {
            var query = @"SELECT Nit, NombreEmpresa, Logo,Email, FechaDeSistema FROM Empresas";
            var listEmpresas = await _db.QueryAsync<Empresas>(query);
            return listEmpresas;
        }

        public async Task<Empresas> GetEmpresaByNitAsync(string nit)
        {
            var exist = await CompanyExistsByNitAsync(nit);
            if (!exist)
            {
                return null;
            }

            var query = @"SELECT Nit, NombreEmpresa,Logo,Email,FechaDeSistema FROM Empresas
                WHERE Nit = @Nit";
            var empresa = await _db.QueryFirstAsync<Empresas>(query, new { Nit = nit });
            return empresa;
        }
    }
}
