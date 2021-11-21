using System.ComponentModel.DataAnnotations;

namespace PeopleBuy.Models
{
    public class Localizacao
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "CEP")]
        public String? CEP { get; set; }
        public String? Logradouro { get; set; }
        [Display(Name ="Número")]
        public String? Numero { get; set; }
        public String? Complemento { get; set; }
        [Display(Name = "Estado")]
        public String? Estado { get; set; }
        public String? Cidade { get; set; }
        public String? Bairro { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
