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
    public class FisicasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FisicasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Fisicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fisica>>> GetFisica()
        {
            return await _context.Fisica.ToListAsync();
        }

        // GET: api/Fisicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fisica>> GetFisica(int id)
        {
            var fisica = await _context.Fisica.FindAsync(id);

            if (fisica == null)
            {
                return NotFound();
            }

            return fisica;
        }

        // PUT: api/Fisicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFisica(int id, Fisica fisica)
        {
            if (id != fisica.ID)
            {
                return BadRequest();
            }

            _context.Entry(fisica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FisicaExists(id))
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

        // POST: api/Fisicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Fisica>> PostFisica(Fisica fisica)
        {
            _context.Fisica.Add(fisica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFisica", new { id = fisica.ID }, fisica);
        }

        // DELETE: api/Fisicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFisica(int id)
        {
            var fisica = await _context.Fisica.FindAsync(id);
            if (fisica == null)
            {
                return NotFound();
            }

            _context.Fisica.Remove(fisica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FisicaExists(int id)
        {
            return _context.Fisica.Any(e => e.ID == id);
        }
    }
}
