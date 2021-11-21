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
    public class ImagensController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImagensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Imagens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Imagem>>> GetImagem()
        {
            return await _context.Imagem.ToListAsync();
        }

        // GET: api/Imagens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Imagem>> GetImagem(int id)
        {
            var imagem = await _context.Imagem.FindAsync(id);

            if (imagem == null)
            {
                return NotFound();
            }

            return imagem;
        }

        // PUT: api/Imagens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagem(int id, Imagem imagem)
        {
            if (id != imagem.ID)
            {
                return BadRequest();
            }

            _context.Entry(imagem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagemExists(id))
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

        // POST: api/Imagens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Imagem>> PostImagem(Imagem imagem)
        {
            _context.Imagem.Add(imagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImagem", new { id = imagem.ID }, imagem);
        }

        // DELETE: api/Imagens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagem(int id)
        {
            var imagem = await _context.Imagem.FindAsync(id);
            if (imagem == null)
            {
                return NotFound();
            }

            _context.Imagem.Remove(imagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImagemExists(int id)
        {
            return _context.Imagem.Any(e => e.ID == id);
        }
    }
}
