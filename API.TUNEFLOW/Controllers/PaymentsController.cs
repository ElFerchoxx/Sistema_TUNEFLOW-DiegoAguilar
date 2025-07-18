﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Tuneflow.Pagos;

namespace API.TUNEFLOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
       /* private readonly TUNEFLOWContext _context;

        public PagosController(TUNEFLOWContext context)
        {
            _context = context;
        */

        private DbConnection connection;
        public PaymentsController(IConfiguration config)
        {
            var connString = config.GetConnectionString("TUNEFLOWContext");
            connection = new Npgsql.NpgsqlConnection(connString);
            connection.Open();
        }

        // GET: api/Pagos
        [HttpGet]
        public IEnumerable<Payment> GetPago()
        {   var payment = connection.Query<Payment>("SELECT * FROM \"Payments\"");
            return payment;
        }

        // GET: api/Pagos/5
        [HttpGet("{id}")]
        public ActionResult<Payment> GetPago(int id)
        {
            var payment = connection.QuerySingle<Payment>(@"SELECT * FROM ""Payments"" WHERE ""Id"" = @Id", new { Id = id });

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        // PUT: api/Pagos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public void PutPago(int id, [FromBody] Payment payment)
        {
            connection.Execute(@"UPDATE ""Payments"" SET 
                ""ClientId"" = @ClientId,
                ""PaymentDate"" = @PaymentDate,
                ""Amount"" = @Amount,
                ""PaymentMethod"" = @PaymentMethod
                WHERE ""Id"" = @Id", new
            {
                Id = id,
                ClientId = payment.ClientId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod
            });
        }

        // POST: api/Pagos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public Payment PostPago([FromBody]Payment payment)
        { connection.Execute(@"INSERT INTO ""Payments"" (""ClientId"", ""PaymentDate"", ""Amount"", ""PaymentMethod"") 
                VALUES (@ClientId, @PaymentDate, @Amount, @PaymentMethod) RETURNING ""Id""", new
        {
            ClientId = payment.ClientId,
            PaymentDate = payment.PaymentDate,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod
        });
            return payment;
        }

        // DELETE: api/Pagos/5
        [HttpDelete("{id}")]
        public void DeletePago(int id)
        {
           connection.Execute(@"DELETE FROM ""Payments"" WERE ""Id"" = @Id", new { Id = id });
           
        }
        /*
        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }*/
    }
}
