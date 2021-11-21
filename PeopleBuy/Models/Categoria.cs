using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class Categoria
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Categoria")]
        public String? Nome { get; set; }
    }
}
