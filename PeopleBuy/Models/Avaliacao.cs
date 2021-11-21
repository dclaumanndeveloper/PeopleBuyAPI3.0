using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class Avaliacao
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Pontuação")]

        public int Pontos { get; set; }
        [Display(Name = "Comentário")]
        public String? Comentario { get; set; }
        [Required]
        public Login? Login { get; set; }
        public int LoginId { get; set; }
        [Required]
        public Oferta? Oferta { get; set; }
        public int IdOferta { get; set; }
    }
}
