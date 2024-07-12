using AutoMapper;
using Dapper;
using System.Data;
using tablero_bi.Domain.Entities;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.Infraestructure.Repositories
{
    public class SucursalRepository : ISucursalRepository
    {

        private readonly IDbConnection _db;

        public SucursalRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<bool> CreateNewSucursalAsync(Sucursales sucursal)
        {
            var query = @"INSERT INTO Sucursales (NombreSucursal, EmpresaId) VALUES (@NombreSucursal, @EmpresaId)";
            var count = await _db.ExecuteAsync(query, new { 
                NombreSucursal = sucursal.NombreSucursal, 
                EmpresaId = sucursal.EmpresaId 
            });

            return count > 0;
        }
    }
}
