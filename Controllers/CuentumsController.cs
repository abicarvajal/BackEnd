using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_db.Models;
using Newtonsoft.Json;

namespace Api_db.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentumsController : ControllerBase
    {
        private readonly bancoContext _context;

        public CuentumsController(bancoContext context)
        {
            _context = context;
        }

        // GET: api/Cuentums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuentum>>> GetCuenta()
        {
          if (_context.Cuenta == null)
          {
              return NotFound();
          }
            return await _context.Cuenta.ToListAsync();
        }

        // GET: api/Cuentums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cuentum>> GetCuentum(int id)
        {
          if (_context.Cuenta == null)
          {
              return NotFound();
          }
            var cuentum = await _context.Cuenta.FindAsync(id);

            if (cuentum == null)
            {
                return NotFound();
            }

            return cuentum;
        }

        // PUT: api/Cuentums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuentum(int id, Cuentum cuentum)
        {
            if (id != cuentum.Idcuenta)
            {
                return BadRequest();
            }

            _context.Entry(cuentum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cuentums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cuentum>> PostCuentum(Cuentum cuentum)
        {
          if (_context.Cuenta == null)
          {
              return Problem("Entity set 'bancoContext.Cuenta'  is null.");
          }
            _context.Cuenta.Add(cuentum);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CuentumExists(cuentum.Idcuenta))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCuentum", new { id = cuentum.Idcuenta }, cuentum);
        }

        // DELETE: api/Cuentums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuentum(int id)
        {
            if (_context.Cuenta == null)
            {
                return NotFound();
            }
            var cuentum = await _context.Cuenta.FindAsync(id);
            if (cuentum == null)
            {
                return NotFound();
            }

            _context.Cuenta.Remove(cuentum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{numCuenta}/{cantidad}")]
        public string transferCash(string numCuenta, int cantidad)
        {
            var exito = "";
            var error = "";
            var result = _context.Cuenta.SingleOrDefault(p => p.Numero == numCuenta);
            if (result!=null)
            {
                try
                {
                    if (result.Saldo - cantidad >= 0)
                    {
                        result.Saldo = result.Saldo - cantidad;
                        _context.SaveChanges();
                        exito = "Saldo modificado";
                    }
                    else
                    {
                        error = "Saldo insuficiente";
                    }

                }
                catch (Exception ex)
                {

                }
            }
            Response resp = new Response(exito, error);
            return JsonConvert.SerializeObject(resp);
        }

        [HttpPut("{numCuenta}/{cant}")]
        public string receiveCash(string numCuenta, int cant)
        {
            var exito = "";
            var error = "";
            var result = _context.Cuenta.SingleOrDefault(p => p.Numero == numCuenta);
            if (result != null)
            {
                try
                {
                    result.Saldo = result.Saldo + cant;
                    _context.SaveChanges();
                    exito = "Saldo modificado";

                }
                catch (Exception ex)
                {
                    error = "Ocurrio un error...";
                }
            }
            Response resp = new Response(exito, error);
            return JsonConvert.SerializeObject(resp);
        }

        private bool CuentumExists(int id)
        {
            return (_context.Cuenta?.Any(e => e.Idcuenta == id)).GetValueOrDefault();
        }
    }
}
