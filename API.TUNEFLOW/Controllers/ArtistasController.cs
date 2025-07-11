﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Tuneflow.Usuario.Consumidor;
using Modelos.Tuneflow.Usuario.Produccion;

namespace API.TUNEFLOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistasController : ControllerBase
    {
        private DbConnection connection;
        /*private readonly TUNEFLOWContext _context;

        public ArtistasController(TUNEFLOWContext context)
        {
            _context = context;
        }*/

        // GET: api/Artistas
        public ArtistasController(IConfiguration config)
        {
            var connString = config.GetConnectionString("TUNEFLOWContext");
            connection = new Npgsql.NpgsqlConnection(connString);
            connection.Open();
        }
        [HttpGet]
        public IEnumerable<Artista> GetArtista()
        {
           var artistas = connection.Query<Artista>("SELECT * FROM \"Artistas\"");
            return artistas;
        }

        // GET: api/Artistas/5
        [HttpGet("{id}")]
        public ActionResult<Artista> GetArtistaById(int id)
        {
            var artista = connection.QuerySingleOrDefault<Artista>(@"SELECT * FROM ""Artistas"" WHERE ""Id"" = @Id", new { Id = id });

            if (artista == null)
            {
                return NotFound();
            }

            return artista;
        }

        // PUT: api/Artistas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public void  PutArtista(int id,[FromBody] Artista artista)
        {
           connection.Execute(@"UPDATE ""Artistas"" SET 
                ""NombreArtistico"" = @NombreArtistico, 
                ""GeneroMusical"" = @GeneroMusical, 
                ""Biografia"" = @Biografia, 
                ""PaisId"" = @PaisId, 
                ""verificado"" = @verificado
                WHERE ""Id"" = @Id", new
           {
               artista.NombreArtistico,
               artista.GeneroMusical,
               artista.Biografia,
               artista.PaisId,
               artista.verificado,
               Id = id
           });
        }

        // POST: api/Artistas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Artista> PostArtista([FromBody]Artista artista)
        {

            var sql = @"INSERT INTO ""Artistas"" 
                (""NombreArtistico"",""GeneroMusical"",""Biografia"",""PaisId"",""verificado"",""Nombre"",""Apellido"",""Email"",""Password"",""Telefono"",""FechaNacimiento"",""TipoCuenta"",""Activo"",""FechaRegistro"",""UsuarioId"") 
                VALUES (@NombreArtistico, @GeneroMusical, @Biografia, @PaisId, @verificado, @Nombre, @Apellido, @Email, @Password, @Telefono, @FechaNacimiento, @TipoCuenta, @Activo, @FechaRegistro, @UsuarioId) RETURNING ""Id"";";
                
            var idDevuelto = connection.ExecuteScalar<int>(sql,new
                {
                    NombreArtistico = artista.NombreArtistico,
                    GeneroMusical = artista.GeneroMusical,
                    Biografia = artista.Biografia,
                    PaisId = artista.PaisId,
                    verificado = artista.verificado,
                    Nombre = artista.Nombre,
                    Apellido = artista.Apellido,
                    Email = artista.Email,
                    Password = artista.Password,
                    Telefono = artista.Telefono,
                    FechaNacimiento = artista.FechaNacimiento,
                    TipoCuenta = artista.TipoCuenta,
                    Activo = artista.Activo,
                    FechaRegistro = artista.FechaRegistro,
                    UsuarioId = artista.UsuarioId
            });

            artista.Id = idDevuelto;

            return CreatedAtAction(nameof(GetArtistaById), new { id = idDevuelto }, artista);
        }

        // DELETE: api/Artistas/5
        [HttpDelete("{id}")]
        public void DeleteArtista(int id)
        { connection.Execute(@"DELETE FROM ""Artistas"" WHERE ""Id"" = @Id", new { Id = id });

        }
/*
        private bool ArtistaExists(int id)
        {
            return _context.Artistas.Any(e => e.Id == id);
        }*/
    }
}
