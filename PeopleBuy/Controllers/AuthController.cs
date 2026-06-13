using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PeopleBuy.Data;
using PeopleBuy.DTOs;
using PeopleBuy.Models;
using PeopleBuy.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PeopleBuy.Controllers
{
    /// <summary>
    /// Gerencia autenticação e cadastro de usuários. Retorna tokens JWT.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Autentica um usuário e retorna um token JWT Bearer.
        /// </summary>
        /// <param name="dto">Credenciais de acesso</param>
        /// <response code="200">Token JWT retornado com sucesso</response>
        /// <response code="401">Credenciais inválidas ou usuário inativo</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
        {
            var login = await _context.Login
                .FirstOrDefaultAsync(l => l.Usuario == dto.Usuario && l.Flag == true);

            if (login == null || !VerifyPassword(dto.Senha, login.Senha!))
                return Unauthorized(new { message = "Credenciais inválidas ou usuário inativo." });

            var expiry = DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("Jwt:ExpiryMinutes", 60));

            return Ok(new LoginResponseDto
            {
                Token = GenerateJwtToken(login, expiry),
                Expiry = expiry,
                TipoLogin = login.TipoLogin.ToString()
            });
        }

        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        /// <param name="dto">Dados de cadastro</param>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="409">Nome de usuário já está em uso</response>
        [HttpPost("register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Login.AnyAsync(l => l.Usuario == dto.Usuario))
                return Conflict(new { message = "Nome de usuário já está em uso." });

            var tipoAcesso = dto.TipoLogin.Equals("Juridica", StringComparison.OrdinalIgnoreCase)
                ? Enumeradores.TipoAcesso.Juridica
                : Enumeradores.TipoAcesso.Física;

            var login = new Login
            {
                Usuario = dto.Usuario,
                Senha = HashPassword(dto.Senha),
                TipoLogin = tipoAcesso,
                Flag = true
            };

            _context.Login.Add(login);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Usuário cadastrado com sucesso.", loginId = login.ID });
        }

        private string GenerateJwtToken(Login login, DateTime expiry)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, login.ID.ToString()),
                new Claim(ClaimTypes.Name, login.Usuario!),
                new Claim(ClaimTypes.Role, login.TipoLogin.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        internal static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, 100_000, HashAlgorithmName.SHA256, 32);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        internal static bool VerifyPassword(string password, string storedHash)
        {
            // Suporte a senhas em texto puro (dados legados) — retorna falso para forçar recadastro
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;

            try
            {
                var salt = Convert.FromBase64String(parts[0]);
                var expectedHash = Convert.FromBase64String(parts[1]);
                var derivedHash = Rfc2898DeriveBytes.Pbkdf2(
                    password, salt, 100_000, HashAlgorithmName.SHA256, 32);
                return CryptographicOperations.FixedTimeEquals(expectedHash, derivedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}
