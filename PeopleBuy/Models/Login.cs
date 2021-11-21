using PeopleBuy.Models.Enums;

using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class Login
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Usuário")]
        public String? Usuario { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Numero de caracteres menor que ou maior que 100 ", MinimumLength = 6)]
        public String? Senha { get; set; }
        [Display(Name = "Tipo de Login")]
        public Enumeradores.TipoAcesso TipoLogin { get; set; }
        [Display(Name = "Situação")]
        public bool Flag { get; set; }

    }
}
