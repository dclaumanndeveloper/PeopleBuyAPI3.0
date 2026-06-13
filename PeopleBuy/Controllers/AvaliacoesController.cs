using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia as avaliações e comentários dos usuários sobre as ofertas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AvaliacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todas as avaliações registradas.</summary>
        /// <response code="200">Lista de avaliações</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Avaliacao>), 200)]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> GetAvaliacao()
        {
            return await _context.Avaliacao.ToListAsync();
        }

        /// <summary>Retorna uma avaliação pelo ID.</summary>
        /// <param name="id">ID da avaliação</param>
        /// <response code="200">Avaliação encontrada</response>
        /// <response code="404">Avaliação não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Avaliacao), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Avaliacao>> GetAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacao.FindAsync(id);
            if (avaliacao == null) return NotFound();
            return avaliacao;
        }

        /// <summary>Atualiza uma avaliação existente.</summary>
        /// <param name="id">ID da avaliação</param>
        /// <param name="avaliacao">Dados atualizados</param>
        /// <response code="204">Atualizado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Avaliação não encontrada</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutAvaliacao(int id, Avaliacao avaliacao)
        {
            if (id != avaliacao.ID) return BadRequest();

            if (avaliacao.Pontos < 0 || avaliacao.Pontos > 5)
                return BadRequest(new { message = "Pontuação deve ser entre 0 e 5." });

            _context.Entry(avaliacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvaliacaoExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Registra uma nova avaliação para uma oferta.</summary>
        /// <param name="avaliacao">Dados da avaliação</param>
        /// <response code="201">Avaliação registrada com sucesso</response>
        /// <response code="400">Pontuação inválida</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Avaliacao), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Avaliacao>> PostAvaliacao(Avaliacao avaliacao)
        {
            if (avaliacao.Pontos < 0 || avaliacao.Pontos > 5)
                return BadRequest(new { message = "Pontuação deve ser entre 0 e 5." });

            _context.Avaliacao.Add(avaliacao);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAvaliacao), new { id = avaliacao.ID }, avaliacao);
        }

        /// <summary>Remove uma avaliação.</summary>
        /// <param name="id">ID da avaliação</param>
        /// <response code="204">Removida com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Avaliação não encontrada</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacao.FindAsync(id);
            if (avaliacao == null) return NotFound();

            _context.Avaliacao.Remove(avaliacao);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool AvaliacaoExists(int id)
        {
            return _context.Avaliacao.Any(e => e.ID == id);
        }
    }
}
