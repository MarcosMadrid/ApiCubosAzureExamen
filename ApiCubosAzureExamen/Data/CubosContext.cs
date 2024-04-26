using ApiCubosAzureExamen.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCubosAzureExamen.Data
{
    public class CubosContext : DbContext
    {
        public CubosContext(DbContextOptions<CubosContext> options) : base(options) { }

        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

    }
}
