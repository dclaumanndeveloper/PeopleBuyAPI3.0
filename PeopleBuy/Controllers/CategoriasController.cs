using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia as categorias de produtos e serviços do marketplace.
    /// Leitura pública; escrita requer autenticação.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todas as categorias disponíveis.</summary>
        /// <response code="200">Lista de categorias</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Categoria>), 200)]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoria()
        {
            return await _context.Categoria.ToListAsync();
        }

        /// <summary>Retorna uma categoria específica pelo ID.</summary>
        /// <param name="id">ID da categoria</param>
        /// <response code="200">Categoria encontrada</response>
        /// <response code="404">Categoria não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Categoria), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null) return NotFound();
            return categoria;
        }

        /// <summary>Atualiza uma categoria.</summary>
        /// <param name="id">ID da categoria</param>
        /// <param name="categoria">Dados atualizados</param>
        /// <response code="204">Atualizado com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Categoria não encontrada</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.ID) return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Cria uma nova categoria.</summary>
        /// <param name="categoria">Dados da categoria</param>
        /// <response code="201">Categoria criada com sucesso</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Categoria), 201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.ID }, categoria);
        }

        /// <summary>Remove uma categoria.</summary>
        /// <param name="id">ID da categoria</param>
        /// <response code="204">Removida com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Categoria não encontrada</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null) return NotFound();

            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.ID == id);
        }
    }
}
