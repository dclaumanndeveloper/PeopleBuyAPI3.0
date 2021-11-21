using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeopleBuy.Models;

namespace PeopleBuy.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PeopleBuy.Models.Login> Login { get; set; }
        public DbSet<PeopleBuy.Models.Categoria> Categoria { get; set; }
        public DbSet<PeopleBuy.Models.SubCategoria> SubCategoria { get; set; }
        public DbSet<PeopleBuy.Models.Fisica> Fisica { get; set; }
        public DbSet<PeopleBuy.Models.Juridica> Juridica { get; set; }
        public DbSet<PeopleBuy.Models.Avaliacao> Avaliacao { get; set; }
        public DbSet<PeopleBuy.Models.Favorito> Favorito { get; set; }
        public DbSet<PeopleBuy.Models.Imagem> Imagem { get; set; }
        public DbSet<PeopleBuy.Models.Oferta> Oferta { get; set; }
        public DbSet<PeopleBuy.Models.OfertaDiaria> OfertaDiaria { get; set; }
    }
}