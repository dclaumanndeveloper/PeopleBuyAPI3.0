using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;
using PeopleBuy.Models.Calculos;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia as ofertas de produtos e serviços publicadas pelas empresas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OfertasController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todas as ofertas disponíveis.</summary>
        /// <response code="200">Lista de ofertas retornada com sucesso</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Oferta>), 200)]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetOferta()
        {
            return await _context.Oferta.ToListAsync();
        }

        /// <summary>Retorna uma oferta específica pelo ID.</summary>
        /// <param name="id">ID da oferta</param>
        /// <response code="200">Oferta encontrada</response>
        /// <response code="404">Oferta não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Oferta), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Oferta>> GetOferta(int id)
        {
            var oferta = await _context.Oferta.FindAsync(id);
            if (oferta == null) return NotFound();
            return oferta;
        }

        /// <summary>
        /// Busca ofertas ativas dentro de um raio de distância a partir de coordenadas geográficas.
        /// </summary>
        /// <param name="latitude">Latitude do ponto de origem (graus decimais)</param>
        /// <param name="longitude">Longitude do ponto de origem (graus decimais)</param>
        /// <param name="raioKm">Raio de busca em quilômetros (padrão: 10 km, máximo: 500 km)</param>
        /// <response code="200">Ofertas próximas encontradas</response>
        /// <response code="400">Parâmetros inválidos</response>
        [HttpGet("proximas")]
        [ProducesResponseType(typeof(IEnumerable<Oferta>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetOfertasProximas(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] double raioKm = 10.0)
        {
            if (raioKm <= 0 || raioKm > 500)
                return BadRequest(new { message = "raioKm deve ser entre 0 e 500." });

            var today = DateTime.Today;
            var ofertas = await _context.Oferta
                .Include(o => o.Juridica)
                    .ThenInclude(j => j!.Localizacao)
                .Where(o => o.FlagAtivo && o.DataInicio <= today && o.DataFim >= today)
                .ToListAsync();

            var proximas = ofertas
                .Where(o => o.Juridica?.Localizacao != null &&
                            GeoLocalizacao.Calculate(
                                latitude, longitude,
                                o.Juridica.Localizacao.Latitude,
                                o.Juridica.Localizacao.Longitude) <= raioKm)
                .ToList();

            return Ok(proximas);
        }

        /// <summary>Atualiza uma oferta existente.</summary>
        /// <param name="id">ID da oferta</param>
        /// <param name="oferta">Dados atualizados da oferta</param>
        /// <response code="204">Oferta atualizada com sucesso</response>
        /// <response code="400">ID não corresponde ao corpo da requisição</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Oferta não encontrada</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutOferta(int id, Oferta oferta)
        {
            if (id != oferta.ID) return BadRequest();

            _context.Entry(oferta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfertaExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Cria uma nova oferta.</summary>
        /// <param name="oferta">Dados da nova oferta</param>
        /// <response code="201">Oferta criada com sucesso</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Oferta), 201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Oferta>> PostOferta(Oferta oferta)
        {
            _context.Oferta.Add(oferta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOferta), new { id = oferta.ID }, oferta);
        }

        /// <summary>Remove uma oferta.</summary>
        /// <param name="id">ID da oferta</param>
        /// <response code="204">Oferta removida com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Oferta não encontrada</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteOferta(int id)
        {
            var oferta = await _context.Oferta.FindAsync(id);
            if (oferta == null) return NotFound();

            _context.Oferta.Remove(oferta);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OfertaExists(int id)
        {
            return _context.Oferta.Any(e => e.ID == id);
        }
    }
}
