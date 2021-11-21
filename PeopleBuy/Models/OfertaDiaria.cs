using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class OfertaDiaria
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public String? Nome { get; set; }

        [DataType(DataType.MultilineText)]
        public String? Descricao { get; set; }
        [Required]

        public float Preco { get; set; }
        [Required]

        public String? Unidade { get; set; }
        [Required]



        public DateTime DataInicio { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataFim { get; set; }
        [Required]
        public Boolean FlagAtivo { get; set; }


        [Required]
        public Categoria? Categoria { get; set; }
        [Required]
        public int CategoriaId { get; set; }

        public String? DiaSemana { get; set; }
        [Required]
        public String? Cep { get; set; }
        [Required]
        public String? Logradouro { get; set; }
        [Required]
        public String? Numero { get; set; }

        public String? Complemento { get; set; }
        [Required]
        public String? Estado { get; set; }
        [Required]
        public String? Cidade { get; set; }
        [Required]
        public String? Bairro { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public String? NomeComercio { get; set; }
    }
}
