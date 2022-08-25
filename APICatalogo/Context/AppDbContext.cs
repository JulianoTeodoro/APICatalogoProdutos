using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Models;

namespace APICatalogo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produto>? Produtos { get; set; }
        public DbSet<Categoria>? Categorias { get; set; }
    }
}
