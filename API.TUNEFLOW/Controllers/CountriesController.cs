﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Tuneflow.Modelos;

namespace API.TUNEFLOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
       /* private readonly TUNEFLOWContext _context;

        public PaisesController(TUNEFLOWContext context)
        {
            _context = context;
        }*/
       private DbConnection connection;
        public CountriesController(IConfiguration config)
        {
            var connString = config.GetConnectionString("TUNEFLOWContext");
            connection = new Npgsql.NpgsqlConnection(connString);
            connection.Open();
        }

        // GET: api/Paises
        [HttpGet]
        public IEnumerable<Country> GetPais()
        {
            var paises = connection.Query<Country>("SELECT * FROM \"Countries\"");
            return paises;
        }

        // GET: api/Paises/5
        [HttpGet("{id}")]
        public ActionResult<Country> GetPais(int id)
        {
            var country = connection.QuerySingle<Country>(@"SELECT * FROM ""Countries"" WHERE ""Id"" = @Id", new { Id = id });

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Paises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public void PutPais(int id,[FromBody] Country country)
        {
            connection.Execute(@"UPDATE ""Countrys"" SET 
              ""Name""= @Name,
              "" Continent""= @Continent,
              ""Currency""= @Currency
            WHERE ""Id"" = @Id", new { Id = id, Name = country.Name, Continent = country.Continent, Currency=country.Currency });
        }

        // POST: api/Paises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public Country PostPais([FromBody]Country country)
        {
           connection.Execute(@"INSERT INTO ""Paises"" (""Name"", ""Continent"", ""Currency"") 
            VALUES (@Name, @Continent, @Currency)", new { Name = country.Name, Continent = country.Continent, Currency = country.Currency });
            return country;
        }

        // DELETE: api/Paises/5
        [HttpDelete("{id}")]
        public void DeletePais(int id)
        {
            connection.Execute(@"DELETE FROM ""Countrys"" WHERE ""Id"" = @Id", new { Id = id });
        }
        /*
        private bool PaisExists(int id)
        {
            return _context.Paises.Any(e => e.Id == id);
        }*/
    }
}
