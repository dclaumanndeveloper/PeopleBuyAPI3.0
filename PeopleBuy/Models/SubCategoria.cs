using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class SubCategoria
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Subcategoria")]
        public String? Nome { get; set; }
        public Categoria? Categoria { get; set; }
        public int CategoriaId { get; set; }
    }
}
