﻿using Dapper;
using System.Data;
using tablero_bi.Domain.Entities;
using tablero_bi.Domain.Interfaces;

namespace tablero_bi.Infraestructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _db;
        public UsuarioRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Usuarios> GetLoginUserAsync(Usuarios usuario)
        {
            var sql = @"
        SELECT 
            u.Username, 
            u.RoleId,
            r.RoleId,
            r.Name,
            e.EmpresaId,
            e.Nit
        FROM 
            Usuarios u
        INNER JOIN 
            Roles r ON u.RoleId = r.RoleId
        INNER JOIN 
            Empresas e ON u.EmpresaId = e.EmpresaId
        WHERE 
            u.Username = @Username
            AND u.Password = @Password
            AND e.Nit = @Nit";

            var lookup = new Dictionary<int, Usuarios>();

            var result = await _db.QueryAsync<Usuarios, Roles, Empresas, Usuarios>(
                sql,
                (usuario, role, empresa) =>
                {
                    if (!lookup.TryGetValue(usuario.UsuarioId, out var userEntry))
                    {
                        userEntry = usuario;
                        userEntry.Roles = role;
                        userEntry.Empresas = empresa;
                        lookup.Add(userEntry.UsuarioId, userEntry);
                    }
                    else
                    {
                        userEntry.Roles = role;
                        userEntry.Empresas = empresa;
                    }
                    return userEntry;
                },
                new { Username = usuario.Username, Password = usuario.Password, Nit = usuario.Empresas.Nit },
                splitOn: "RoleId,EmpresaId",
                commandType: CommandType.Text); // Cambia a CommandType.StoredProcedure si es un procedimiento almacenado

            return lookup.Values.FirstOrDefault();
        }

        public async Task<Usuarios> GetUserByUsername(string username)
        {
            var sql = @"
                SELECT u.Username, r.Name, e.Nit
                FROM usuarios u
                INNER JOIN Roles r ON u.RoleId = r.RoleId
                INNER JOIN Empresas e ON u.EmpresaId = e.EmpresaId
                WHERE u.Username = @Username";

            var result = await _db.QueryAsync<Usuarios, Roles, Empresas, Usuarios>(
        sql,
        (usuario, role, empresa) =>
        {
            usuario.Roles = role; // Asigna el rol directamente al usuario
            usuario.Empresas = empresa; // Asigna la empresa directamente al usuario
            return usuario;
        },
        new { Username = username },
        splitOn: "Name,Nit");

            return result.FirstOrDefault();

        }

    }
}
