using PeopleBuy.Controllers;
using Xunit;

namespace PeopleBuy.Tests
{
    public class AuthHelperTests
    {
        [Fact]
        public void HashPassword_GeraHashDiferenteDaSenha()
        {
            var senha = "minhasenha123";
            var hash = AuthController.HashPassword(senha);
            Assert.NotEqual(senha, hash);
        }

        [Fact]
        public void HashPassword_MesmoInputGeraHashesDiferentes()
        {
            // PBKDF2 usa salt aleatório — dois hashes da mesma senha devem ser diferentes
            var hash1 = AuthController.HashPassword("senha123");
            var hash2 = AuthController.HashPassword("senha123");
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_SenhaCorreta_RetornaTrue()
        {
            var senha = "senhaSegura!456";
            var hash = AuthController.HashPassword(senha);
            Assert.True(AuthController.VerifyPassword(senha, hash));
        }

        [Fact]
        public void VerifyPassword_SenhaErrada_RetornaFalse()
        {
            var hash = AuthController.HashPassword("senhaCorreta");
            Assert.False(AuthController.VerifyPassword("senhaErrada", hash));
        }

        [Fact]
        public void VerifyPassword_HashLegadoSemFormato_RetornaFalse()
        {
            // Senhas em texto puro (formato antigo) devem ser rejeitadas
            Assert.False(AuthController.VerifyPassword("123456", "123456"));
        }

        [Fact]
        public void HashPassword_FormatoContemDoisPartes()
        {
            var hash = AuthController.HashPassword("qualquersenha");
            var partes = hash.Split(':');
            Assert.Equal(2, partes.Length);
        }
    }
}
