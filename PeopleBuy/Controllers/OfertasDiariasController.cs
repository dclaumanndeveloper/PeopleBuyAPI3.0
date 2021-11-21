using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;

namespace PeopleBuy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasDiariasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OfertasDiariasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OfertasDiarias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfertaDiaria>>> GetOfertaDiaria()
        {
            return await _context.OfertaDiaria.ToListAsync();
        }

        // GET: api/OfertasDiarias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfertaDiaria>> GetOfertaDiaria(int id)
        {
            var ofertaDiaria = await _context.OfertaDiaria.FindAsync(id);

            if (ofertaDiaria == null)
            {
                return NotFound();
            }

            return ofertaDiaria;
        }

        // PUT: api/OfertasDiarias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfertaDiaria(int id, OfertaDiaria ofertaDiaria)
        {
            if (id != ofertaDiaria.ID)
            {
                return BadRequest();
            }

            _context.Entry(ofertaDiaria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfertaDiariaExists(id))
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

        // POST: api/OfertasDiarias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OfertaDiaria>> PostOfertaDiaria(OfertaDiaria ofertaDiaria)
        {
            _context.OfertaDiaria.Add(ofertaDiaria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOfertaDiaria", new { id = ofertaDiaria.ID }, ofertaDiaria);
        }

        // DELETE: api/OfertasDiarias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfertaDiaria(int id)
        {
            var ofertaDiaria = await _context.OfertaDiaria.FindAsync(id);
            if (ofertaDiaria == null)
            {
                return NotFound();
            }

            _context.OfertaDiaria.Remove(ofertaDiaria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfertaDiariaExists(int id)
        {
            return _context.OfertaDiaria.Any(e => e.ID == id);
        }
    }
}
