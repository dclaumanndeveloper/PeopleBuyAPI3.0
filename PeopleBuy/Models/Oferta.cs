using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class Oferta
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public String? Nome { get; set; }
        [Display(Name = "Descrição")]
        [DataType(DataType.MultilineText)]
        public String? Descricao { get; set; }
        [Required]
        [Display(Name = "Preço")]
        public float Preco { get; set; }
        [Required]
        [Display(Name = "Unidade")]
        public String? Unidade { get; set; }
        [Required]

        [Display(Name = "Início")]

        public DateTime DataInicio { get; set; }
        [Display(Name = "Data Final")]
        [DataType(DataType.DateTime)]
        public DateTime DataFim { get; set; }
        public Boolean FlagAtivo { get; set; }

        public String? NomeImagem { get; set; }
        public Imagem? Imagem { get; set; }
        public int ImagemId { get; set; }
        [Required]
        public Categoria? Categoria { get; set; }
        public int CategoriaId { get; set; }

        [Required]
        public Juridica? Juridica { get; set; }
        public int IdJuridica { get; set; }
    }
}
