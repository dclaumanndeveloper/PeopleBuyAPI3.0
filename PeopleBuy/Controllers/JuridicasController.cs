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
    public class JuridicasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JuridicasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Juridicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Juridica>>> GetJuridica()
        {
            return await _context.Juridica.ToListAsync();
        }

        // GET: api/Juridicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Juridica>> GetJuridica(int id)
        {
            var juridica = await _context.Juridica.FindAsync(id);

            if (juridica == null)
            {
                return NotFound();
            }

            return juridica;
        }

        // PUT: api/Juridicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJuridica(int id, Juridica juridica)
        {
            if (id != juridica.ID)
            {
                return BadRequest();
            }

            _context.Entry(juridica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JuridicaExists(id))
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

        // POST: api/Juridicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Juridica>> PostJuridica(Juridica juridica)
        {
            _context.Juridica.Add(juridica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJuridica", new { id = juridica.ID }, juridica);
        }

        // DELETE: api/Juridicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJuridica(int id)
        {
            var juridica = await _context.Juridica.FindAsync(id);
            if (juridica == null)
            {
                return NotFound();
            }

            _context.Juridica.Remove(juridica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JuridicaExists(int id)
        {
            return _context.Juridica.Any(e => e.ID == id);
        }
    }
}
