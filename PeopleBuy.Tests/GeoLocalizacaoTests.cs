using PeopleBuy.Models.Calculos;
using Xunit;

namespace PeopleBuy.Tests
{
    public class GeoLocalizacaoTests
    {
        [Fact]
        public void Calculate_MesmosPontos_RetornaZero()
        {
            var resultado = GeoLocalizacao.Calculate(-23.5505, -46.6333, -23.5505, -46.6333);
            Assert.Equal(0.0, resultado, precision: 5);
        }

        [Fact]
        public void Calculate_SaoPauloParaRioDeJaneiro_DistanciaAproximada()
        {
            // SP (-23.5505, -46.6333) → RJ (-22.9068, -43.1729) ≈ 357 km
            var resultado = GeoLocalizacao.Calculate(-23.5505, -46.6333, -22.9068, -43.1729);
            Assert.InRange(resultado, 340.0, 380.0);
        }

        [Fact]
        public void Calculate_SaoPauloParaBrasilia_DistanciaAproximada()
        {
            // SP (-23.5505, -46.6333) → BSB (-15.7942, -47.8825) ≈ 873 km
            var resultado = GeoLocalizacao.Calculate(-23.5505, -46.6333, -15.7942, -47.8825);
            Assert.InRange(resultado, 850.0, 900.0);
        }

        [Fact]
        public void Calculate_DistanciaEhSimetrica()
        {
            var ida = GeoLocalizacao.Calculate(-23.5505, -46.6333, -22.9068, -43.1729);
            var volta = GeoLocalizacao.Calculate(-22.9068, -43.1729, -23.5505, -46.6333);
            Assert.Equal(ida, volta, precision: 5);
        }

        [Fact]
        public void Calculate_ValoresPositivos_SempreMaiorQueZero()
        {
            var resultado = GeoLocalizacao.Calculate(-23.5505, -46.6333, -22.9068, -43.1729);
            Assert.True(resultado > 0);
        }
    }
}
