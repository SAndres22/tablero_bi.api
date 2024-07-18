﻿using Dapper;
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

        public async Task<Sucursales> GetSucursalByIdAsync(int id)
        {
            var query = @"SELECT * FROM Sucursales WHERE SucursalId = @Id";
            var sucursal = await _db.QueryFirstAsync<Sucursales>(query, new { Id = id });
            return sucursal;

        }

        public async Task<IEnumerable<Sucursales>> GetSucursalesAsync(string nitEmpresa)
        {
            var query = @"SELECT DISTINCT s.SucursalId,s.NombreSucursal, s.EmpresaId
                        FROM Sucursales s
                        INNER JOIN Empresas e ON s.EmpresaId = e.EmpresaId
                        WHERE e.Nit = @NitEmpresa";

            var listaSucrsales = await _db.QueryAsync<Sucursales>(query, new {NitEmpresa =  nitEmpresa});
            return listaSucrsales.ToList();
        }

        public async Task<bool> SucursalAsociadaAEmpresaAsync(string nitEmpresa, int idsucursal)
        {
            var query = @"SELECT COUNT(1)
                FROM Sucursales S
                INNER JOIN Empresas E ON S.EmpresaId = E.EmpresaId
                WHERE E.Nit = @NitEmpresa
                AND S.SucursalId = @IdSucursal";

            var count = await _db.ExecuteScalarAsync<int>(query, new { 
                NitEmpresa = nitEmpresa, IdSucursal = idsucursal 
            });

            return count > 0;
        }
    }
}
