using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryClasses
{
    public class InventoryContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Shelf> Shelves { get; set; }

        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("your_connection_string")
                              .LogTo(Console.WriteLine, LogLevel.Information); // Sprawdź tę linię i zmień LogLevel
            }
        }
    }
}
