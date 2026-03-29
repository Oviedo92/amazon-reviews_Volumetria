using AmazonAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AmazonAPI.Data
{
    public class AmazonDbContext : DbContext
    {
        // El constructor público y vacío que necesita C# para inyectar la conexión
        public AmazonDbContext(DbContextOptions<AmazonDbContext> options) : base(options)
        {
        }

        // Tus dos ejércitos de datos listos para ser consultados
        public DbSet<Resena> Resenas { get; set; }
        public DbSet<Metadata> Metadatas { get; set; }
    }
}