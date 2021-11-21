using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models.Base
{
    public class Pessoa
    {
        [Display(Name = "Celular")]
        [DataType(DataType.PhoneNumber)]
        public String? Celular { get; set; }
        [DataType(DataType.EmailAddress)]
        public String? Email { get; set; }
    }
}
