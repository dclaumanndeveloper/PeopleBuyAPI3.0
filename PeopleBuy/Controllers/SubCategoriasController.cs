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
    public class SubCategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SubCategorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubCategoria>>> GetSubCategoria()
        {
            return await _context.SubCategoria.ToListAsync();
        }

        // GET: api/SubCategorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubCategoria>> GetSubCategoria(int id)
        {
            var subCategoria = await _context.SubCategoria.FindAsync(id);

            if (subCategoria == null)
            {
                return NotFound();
            }

            return subCategoria;
        }

        // PUT: api/SubCategorias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubCategoria(int id, SubCategoria subCategoria)
        {
            if (id != subCategoria.ID)
            {
                return BadRequest();
            }

            _context.Entry(subCategoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCategoriaExists(id))
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

        // POST: api/SubCategorias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubCategoria>> PostSubCategoria(SubCategoria subCategoria)
        {
            _context.SubCategoria.Add(subCategoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubCategoria", new { id = subCategoria.ID }, subCategoria);
        }

        // DELETE: api/SubCategorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategoria(int id)
        {
            var subCategoria = await _context.SubCategoria.FindAsync(id);
            if (subCategoria == null)
            {
                return NotFound();
            }

            _context.SubCategoria.Remove(subCategoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubCategoriaExists(int id)
        {
            return _context.SubCategoria.Any(e => e.ID == id);
        }
    }
}
