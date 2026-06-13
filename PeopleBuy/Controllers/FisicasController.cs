using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;
using PeopleBuy.Models.Validadores;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia o cadastro de pessoas físicas (clientes individuais) na plataforma.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FisicasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FisicasController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todos os cadastros de pessoas físicas.</summary>
        /// <response code="200">Lista de pessoas físicas</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Fisica>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<Fisica>>> GetFisica()
        {
            return await _context.Fisica.ToListAsync();
        }

        /// <summary>Retorna uma pessoa física pelo ID.</summary>
        /// <param name="id">ID do cadastro</param>
        /// <response code="200">Pessoa física encontrada</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrada</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Fisica), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Fisica>> GetFisica(int id)
        {
            var fisica = await _context.Fisica.FindAsync(id);
            if (fisica == null) return NotFound();
            return fisica;
        }

        /// <summary>Atualiza o cadastro de uma pessoa física.</summary>
        /// <param name="id">ID do cadastro</param>
        /// <param name="fisica">Dados atualizados</param>
        /// <response code="204">Atualizado com sucesso</response>
        /// <response code="400">CPF inválido ou ID incorreto</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrada</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutFisica(int id, Fisica fisica)
        {
            if (id != fisica.ID) return BadRequest();

            if (fisica.CPF != null && !CPF.IsCpf(fisica.CPF))
                return BadRequest(new { message = "CPF inválido." });

            _context.Entry(fisica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FisicaExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Cadastra uma nova pessoa física.</summary>
        /// <param name="fisica">Dados da pessoa física</param>
        /// <response code="201">Cadastro criado com sucesso</response>
        /// <response code="400">CPF inválido</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Fisica), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Fisica>> PostFisica(Fisica fisica)
        {
            if (fisica.CPF != null && !CPF.IsCpf(fisica.CPF))
                return BadRequest(new { message = "CPF inválido." });

            _context.Fisica.Add(fisica);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFisica), new { id = fisica.ID }, fisica);
        }

        /// <summary>Remove o cadastro de uma pessoa física.</summary>
        /// <param name="id">ID do cadastro</param>
        /// <response code="204">Removido com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrada</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteFisica(int id)
        {
            var fisica = await _context.Fisica.FindAsync(id);
            if (fisica == null) return NotFound();

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
