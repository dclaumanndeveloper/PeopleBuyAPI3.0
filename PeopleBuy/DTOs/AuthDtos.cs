using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.DTOs
{
    /// <summary>Dados para autenticação de usuário.</summary>
    public class LoginRequestDto
    {
        /// <summary>Nome de usuário</summary>
        [Required]
        public string Usuario { get; set; } = string.Empty;

        /// <summary>Senha do usuário</summary>
        [Required]
        public string Senha { get; set; } = string.Empty;
    }

    /// <summary>Resposta com token JWT após autenticação bem-sucedida.</summary>
    public class LoginResponseDto
    {
        /// <summary>Token JWT para uso nas requisições protegidas</summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>Data/hora de expiração do token (UTC)</summary>
        public DateTime Expiry { get; set; }

        /// <summary>Tipo de acesso: Física ou Juridica</summary>
        public string TipoLogin { get; set; } = string.Empty;
    }

    /// <summary>Dados para cadastro de novo usuário.</summary>
    public class RegisterDto
    {
        /// <summary>Nome de usuário (único no sistema)</summary>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Usuario { get; set; } = string.Empty;

        /// <summary>Senha (mínimo 6 caracteres)</summary>
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Senha { get; set; } = string.Empty;

        /// <summary>Tipo de acesso: "Fisica" ou "Juridica"</summary>
        [Required]
        public string TipoLogin { get; set; } = "Fisica";
    }
}
