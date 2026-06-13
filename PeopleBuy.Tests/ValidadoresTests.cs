using PeopleBuy.Models.Validadores;
using Xunit;

namespace PeopleBuy.Tests
{
    public class CpfValidadorTests
    {
        [Theory]
        [InlineData("529.982.247-25", true)]
        [InlineData("111.444.777-35", true)]
        [InlineData("000.000.000-00", false)]
        [InlineData("123.456.789-99", false)]
        [InlineData("111.111.111-11", false)]
        [InlineData("abc.def.ghi-jk", false)]
        [InlineData("", false)]
        public void IsCpf_RetornaResultadoEsperado(string cpf, bool esperado)
        {
            Assert.Equal(esperado, CPF.IsCpf(cpf));
        }
    }

    public class CnpjValidadorTests
    {
        [Theory]
        [InlineData("11.222.333/0001-81", true)]
        [InlineData("45.543.915/0001-04", true)]
        [InlineData("00.000.000/0000-00", false)]
        [InlineData("11.111.111/1111-11", false)]
        [InlineData("12.345.678/0001-99", false)]
        [InlineData("", false)]
        public void IsCnpj_RetornaResultadoEsperado(string cnpj, bool esperado)
        {
            Assert.Equal(esperado, CNPJ.IsCnpj(cnpj));
        }
    }
}
