using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia a lista de favoritos dos usuários.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FavoritosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todos os favoritos (do usuário autenticado).</summary>
        /// <response code="200">Lista de favoritos</response>
        /// <response code="401">Não autenticado</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Favorito>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<Favorito>>> GetFavorito()
        {
            return await _context.Favorito.ToListAsync();
        }

        /// <summary>Retorna um favorito pelo ID.</summary>
        /// <param name="id">ID do favorito</param>
        /// <response code="200">Favorito encontrado</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Favorito não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Favorito), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Favorito>> GetFavorito(int id)
        {
            var favorito = await _context.Favorito.FindAsync(id);
            if (favorito == null) return NotFound();
            return favorito;
        }

        /// <summary>Atualiza um favorito.</summary>
        /// <param name="id">ID do favorito</param>
        /// <param name="favorito">Dados atualizados</param>
        /// <response code="204">Atualizado com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Favorito não encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutFavorito(int id, Favorito favorito)
        {
            if (id != favorito.ID) return BadRequest();

            _context.Entry(favorito).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoritoExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Adiciona uma oferta aos favoritos.</summary>
        /// <param name="favorito">Dados do favorito</param>
        /// <response code="201">Adicionado aos favoritos</response>
        /// <response code="401">Não autenticado</response>
        [HttpPost]
        [ProducesResponseType(typeof(Favorito), 201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Favorito>> PostFavorito(Favorito favorito)
        {
            _context.Favorito.Add(favorito);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFavorito), new { id = favorito.ID }, favorito);
        }

        /// <summary>Remove um favorito.</summary>
        /// <param name="id">ID do favorito</param>
        /// <response code="204">Removido com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Favorito não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteFavorito(int id)
        {
            var favorito = await _context.Favorito.FindAsync(id);
            if (favorito == null) return NotFound();

            _context.Favorito.Remove(favorito);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool FavoritoExists(int id)
        {
            return _context.Favorito.Any(e => e.ID == id);
        }
    }
}
