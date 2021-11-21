namespace PeopleBuy.Models
{
    public class Favorito
    {
        public int ID { get; set; }
        public Oferta? Oferta { get; set; }
        public int OfertaId { get; set; }
        public Login? Login { get; set; }
        public int LoginId { get; set; }
        public bool Flag { get; set; }
    }
}
