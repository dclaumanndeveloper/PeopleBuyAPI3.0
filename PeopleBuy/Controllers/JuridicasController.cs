using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;
using PeopleBuy.Models.Validadores;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia o cadastro de pessoas jurídicas (empresas) que publicam ofertas na plataforma.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JuridicasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JuridicasController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todos os cadastros de pessoas jurídicas.</summary>
        /// <response code="200">Lista de pessoas jurídicas</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Juridica>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<Juridica>>> GetJuridica()
        {
            return await _context.Juridica.ToListAsync();
        }

        /// <summary>Retorna uma pessoa jurídica pelo ID.</summary>
        /// <param name="id">ID do cadastro</param>
        /// <response code="200">Empresa encontrada</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrada</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Juridica), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Juridica>> GetJuridica(int id)
        {
            var juridica = await _context.Juridica.FindAsync(id);
            if (juridica == null) return NotFound();
            return juridica;
        }

        /// <summary>Atualiza o cadastro de uma pessoa jurídica.</summary>
        /// <param name="id">ID do cadastro</param>
        /// <param name="juridica">Dados atualizados</param>
        /// <response code="204">Atualizado com sucesso</response>
        /// <response code="400">CNPJ inválido ou ID incorreto</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrada</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutJuridica(int id, Juridica juridica)
        {
            if (id != juridica.ID) return BadRequest();

            if (juridica.CNPJ != null && !CNPJ.IsCnpj(juridica.CNPJ))
                return BadRequest(new { message = "CNPJ inválido." });

            _context.Entry(juridica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JuridicaExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Cadastra uma nova empresa.</summary>
        /// <param name="juridica">Dados da empresa</param>
        /// <response code="201">Cadastro criado com sucesso</response>
        /// <response code="400">CNPJ inválido</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Juridica), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Juridica>> PostJuridica(Juridica juridica)
        {
            if (juridica.CNPJ != null && !CNPJ.IsCnpj(juridica.CNPJ))
                return BadRequest(new { message = "CNPJ inválido." });

            _context.Juridica.Add(juridica);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJuridica), new { id = juridica.ID }, juridica);
        }

        /// <summary>Remove o cadastro de uma empresa.</summary>
        /// <param name="id">ID do cadastro</param>
        /// <response code="204">Removido com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrada</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteJuridica(int id)
        {
            var juridica = await _context.Juridica.FindAsync(id);
            if (juridica == null) return NotFound();

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
