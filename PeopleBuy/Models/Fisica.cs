using PeopleBuy.Models.Base;

using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class Fisica : Pessoa
    {
        public int ID { get; set; }

        [Display(Name = "CPF")]

        public String? CPF { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public String? NomePessoa { get; set; }
        [Required]
        public Login? Login { get; set; }
        public int LoginId { get; set; }
    }
}
