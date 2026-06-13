using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia as subcategorias vinculadas às categorias do marketplace.
    /// Leitura pública; escrita requer autenticação.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todas as subcategorias disponíveis.</summary>
        /// <response code="200">Lista de subcategorias</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubCategoria>), 200)]
        public async Task<ActionResult<IEnumerable<SubCategoria>>> GetSubCategoria()
        {
            return await _context.SubCategoria.ToListAsync();
        }

        /// <summary>Retorna uma subcategoria pelo ID.</summary>
        /// <param name="id">ID da subcategoria</param>
        /// <response code="200">Subcategoria encontrada</response>
        /// <response code="404">Subcategoria não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SubCategoria), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SubCategoria>> GetSubCategoria(int id)
        {
            var subCategoria = await _context.SubCategoria.FindAsync(id);
            if (subCategoria == null) return NotFound();
            return subCategoria;
        }

        /// <summary>Atualiza uma subcategoria.</summary>
        /// <param name="id">ID da subcategoria</param>
        /// <param name="subCategoria">Dados atualizados</param>
        /// <response code="204">Atualizado com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Subcategoria não encontrada</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutSubCategoria(int id, SubCategoria subCategoria)
        {
            if (id != subCategoria.ID) return BadRequest();

            _context.Entry(subCategoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCategoriaExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Cria uma nova subcategoria.</summary>
        /// <param name="subCategoria">Dados da subcategoria</param>
        /// <response code="201">Subcategoria criada com sucesso</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(SubCategoria), 201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<SubCategoria>> PostSubCategoria(SubCategoria subCategoria)
        {
            _context.SubCategoria.Add(subCategoria);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSubCategoria), new { id = subCategoria.ID }, subCategoria);
        }

        /// <summary>Remove uma subcategoria.</summary>
        /// <param name="id">ID da subcategoria</param>
        /// <response code="204">Removida com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Subcategoria não encontrada</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSubCategoria(int id)
        {
            var subCategoria = await _context.SubCategoria.FindAsync(id);
            if (subCategoria == null) return NotFound();

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
