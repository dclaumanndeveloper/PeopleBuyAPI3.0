using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class Imagem
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nome da imagem")]
        public String? Nome { get; set; }
        public String? Extensao {get;set;}
    }
}
