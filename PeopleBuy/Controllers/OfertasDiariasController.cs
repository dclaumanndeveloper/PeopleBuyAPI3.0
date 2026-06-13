using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;
using PeopleBuy.Models.Calculos;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia as ofertas diárias com localização geográfica embutida.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasDiariasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OfertasDiariasController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todas as ofertas diárias.</summary>
        /// <response code="200">Lista de ofertas diárias</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OfertaDiaria>), 200)]
        public async Task<ActionResult<IEnumerable<OfertaDiaria>>> GetOfertaDiaria()
        {
            return await _context.OfertaDiaria.ToListAsync();
        }

        /// <summary>Retorna uma oferta diária específica pelo ID.</summary>
        /// <param name="id">ID da oferta diária</param>
        /// <response code="200">Oferta diária encontrada</response>
        /// <response code="404">Oferta diária não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OfertaDiaria), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OfertaDiaria>> GetOfertaDiaria(int id)
        {
            var ofertaDiaria = await _context.OfertaDiaria.FindAsync(id);
            if (ofertaDiaria == null) return NotFound();
            return ofertaDiaria;
        }

        /// <summary>
        /// Busca ofertas diárias ativas próximas a uma localização geográfica.
        /// </summary>
        /// <param name="latitude">Latitude do ponto de origem (graus decimais)</param>
        /// <param name="longitude">Longitude do ponto de origem (graus decimais)</param>
        /// <param name="raioKm">Raio de busca em quilômetros (padrão: 10 km, máximo: 500 km)</param>
        /// <response code="200">Ofertas diárias próximas encontradas</response>
        /// <response code="400">Parâmetros inválidos</response>
        [HttpGet("proximas")]
        [ProducesResponseType(typeof(IEnumerable<OfertaDiaria>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<OfertaDiaria>>> GetOfertasDiariasProximas(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] double raioKm = 10.0)
        {
            if (raioKm <= 0 || raioKm > 500)
                return BadRequest(new { message = "raioKm deve ser entre 0 e 500." });

            var today = DateTime.Today;
            var ofertasAtivas = await _context.OfertaDiaria
                .Where(o => o.FlagAtivo && o.DataInicio <= today && o.DataFim >= today)
                .ToListAsync();

            var proximas = ofertasAtivas
                .Where(o => GeoLocalizacao.Calculate(
                    latitude, longitude, o.Latitude, o.Longitude) <= raioKm)
                .ToList();

            return Ok(proximas);
        }

        /// <summary>Atualiza uma oferta diária existente.</summary>
        /// <param name="id">ID da oferta diária</param>
        /// <param name="ofertaDiaria">Dados atualizados</param>
        /// <response code="204">Atualizado com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrada</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutOfertaDiaria(int id, OfertaDiaria ofertaDiaria)
        {
            if (id != ofertaDiaria.ID) return BadRequest();

            _context.Entry(ofertaDiaria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfertaDiariaExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Cria uma nova oferta diária.</summary>
        /// <param name="ofertaDiaria">Dados da nova oferta diária</param>
        /// <response code="201">Criada com sucesso</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(OfertaDiaria), 201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OfertaDiaria>> PostOfertaDiaria(OfertaDiaria ofertaDiaria)
        {
            _context.OfertaDiaria.Add(ofertaDiaria);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOfertaDiaria), new { id = ofertaDiaria.ID }, ofertaDiaria);
        }

        /// <summary>Remove uma oferta diária.</summary>
        /// <param name="id">ID da oferta diária</param>
        /// <response code="204">Removida com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrada</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteOfertaDiaria(int id)
        {
            var ofertaDiaria = await _context.OfertaDiaria.FindAsync(id);
            if (ofertaDiaria == null) return NotFound();

            _context.OfertaDiaria.Remove(ofertaDiaria);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OfertaDiariaExists(int id)
        {
            return _context.OfertaDiaria.Any(e => e.ID == id);
        }
    }
}
