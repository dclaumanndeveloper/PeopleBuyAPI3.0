using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Data;
using PeopleBuy.Models;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia os registros de login (credenciais) dos usuários.
    /// Para autenticar, use POST /api/auth/login.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Retorna todos os registros de login (sem expor a senha).</summary>
        /// <response code="200">Lista de logins</response>
        /// <response code="401">Não autenticado</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Login>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<Login>>> GetLogin()
        {
            return await _context.Login
                .Select(l => new Login
                {
                    ID = l.ID,
                    Usuario = l.Usuario,
                    Senha = "***",
                    TipoLogin = l.TipoLogin,
                    Flag = l.Flag
                })
                .ToListAsync();
        }

        /// <summary>Retorna um registro de login pelo ID.</summary>
        /// <param name="id">ID do login</param>
        /// <response code="200">Login encontrado</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Login não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Login), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Login>> GetLogin(int id)
        {
            var login = await _context.Login.FindAsync(id);
            if (login == null) return NotFound();
            login.Senha = "***";
            return login;
        }

        /// <summary>Atualiza um registro de login.</summary>
        /// <param name="id">ID do login</param>
        /// <param name="login">Dados atualizados</param>
        /// <response code="204">Atualizado com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutLogin(int id, Login login)
        {
            if (id != login.ID) return BadRequest();

            if (!string.IsNullOrWhiteSpace(login.Senha) && login.Senha != "***")
                login.Senha = AuthController.HashPassword(login.Senha);

            _context.Entry(login).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Cria um novo registro de login (use /api/auth/register para cadastro completo).</summary>
        /// <param name="login">Dados do login</param>
        /// <response code="201">Login criado com sucesso</response>
        /// <response code="401">Não autenticado</response>
        [HttpPost]
        [ProducesResponseType(typeof(Login), 201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Login>> PostLogin(Login login)
        {
            if (!string.IsNullOrWhiteSpace(login.Senha))
                login.Senha = AuthController.HashPassword(login.Senha);

            _context.Login.Add(login);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLogin), new { id = login.ID }, login);
        }

        /// <summary>Remove um registro de login.</summary>
        /// <param name="id">ID do login</param>
        /// <response code="204">Removido com sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="404">Não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteLogin(int id)
        {
            var login = await _context.Login.FindAsync(id);
            if (login == null) return NotFound();

            _context.Login.Remove(login);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool LoginExists(int id)
        {
            return _context.Login.Any(e => e.ID == id);
        }
    }
}
