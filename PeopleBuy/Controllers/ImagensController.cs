using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia imagens associadas às ofertas da plataforma.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImagensController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        public ImagensController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>Retorna todos os registros de imagens.</summary>
        /// <response code="200">Lista de imagens</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Imagem>), 200)]
        public async Task<ActionResult<IEnumerable<Imagem>>> GetImagem()
        {
            return await _context.Imagem.ToListAsync();
        }

        /// <summary>Retorna uma imagem específica pelo ID.</summary>
        /// <param name="id">ID da imagem</param>
        /// <response code="200">Imagem encontrada</response>
        /// <response code="404">Imagem não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Imagem), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Imagem>> GetImagem(int id)
        {
            var imagem = await _context.Imagem.FindAsync(id);
            if (imagem == null) return NotFound();
            return imagem;
        }

        /// <summary>
        /// Faz upload de um arquivo de imagem e registra os metadados no banco.
        /// Formatos aceitos: jpg, jpeg, png, webp. Tamanho máximo: 5 MB.
        /// </summary>
        /// <param name="arquivo">Arquivo de imagem (multipart/form-data)</param>
        /// <response code="201">Imagem enviada e registrada com sucesso</response>
        /// <response code="400">Arquivo inválido (extensão não permitida ou tamanho excedido)</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpPost("upload")]
        [ProducesResponseType(typeof(Imagem), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Imagem>> UploadImagem(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest(new { message = "Nenhum arquivo enviado." });

            var ext = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                return BadRequest(new { message = "Extensão não permitida. Use: jpg, jpeg, png ou webp." });

            if (arquivo.Length > MaxFileSizeBytes)
                return BadRequest(new { message = "Arquivo excede o tamanho máximo de 5 MB." });

            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);

            var uniqueName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsPath, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var imagem = new Imagem
            {
                Nome = Path.GetFileNameWithoutExtension(arquivo.FileName),
                Extensao = ext,
                CaminhoArquivo = $"/uploads/{uniqueName}"
            };

            _context.Imagem.Add(imagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImagem), new { id = imagem.ID }, imagem);
        }

        /// <summary>Atualiza os metadados de uma imagem.</summary>
        /// <param name="id">ID da imagem</param>
        /// <param name="imagem">Dados atualizados</param>
        /// <response code="204">Atualizado com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Imagem não encontrada</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutImagem(int id, Imagem imagem)
        {
            if (id != imagem.ID) return BadRequest();

            _context.Entry(imagem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagemExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Registra metadados de uma imagem manualmente (sem upload de arquivo).</summary>
        /// <param name="imagem">Metadados da imagem</param>
        /// <response code="201">Registro criado com sucesso</response>
        /// <response code="401">Não autenticado</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Imagem), 201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Imagem>> PostImagem(Imagem imagem)
        {
            _context.Imagem.Add(imagem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetImagem), new { id = imagem.ID }, imagem);
        }

        /// <summary>Remove o registro de uma imagem.</summary>
        /// <param name="id">ID da imagem</param>
        /// <response code="204">Removido com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Imagem não encontrada</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteImagem(int id)
        {
            var imagem = await _context.Imagem.FindAsync(id);
            if (imagem == null) return NotFound();

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
