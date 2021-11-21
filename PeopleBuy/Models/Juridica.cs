using PeopleBuy.Models.Base;

using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class Juridica: Pessoa
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "CNPJ")]
        public String? CNPJ { get; set; }
        [Display(Name = "Razão Social")]
        public String? RazaoSocial { get; set; }


        [Display(Name = "Telefone")]
        public String? Telefone { get; set; }
        public Login? Login { get; set; }
        public int IdLogin { get; set; }
        public Localizacao? Localizacao { get; set; }
        public int localizacaoId { get; set; }

    }
}
