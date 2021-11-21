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
    }
}