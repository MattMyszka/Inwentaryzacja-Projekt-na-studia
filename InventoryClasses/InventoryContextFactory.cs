using InventoryClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Inwentaryzacja
{
    public class InventoryContextFactory:
    IDesignTimeDbContextFactory<InventoryContext>
    {
        public InventoryContext CreateDbContext(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "..", "..", "..", "database_setting.json");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile(filePath)
                .Build();

            var builder = new DbContextOptionsBuilder<InventoryContext>();
            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            builder.UseNpgsql(connectionString);

            return new InventoryContext(builder.Options);
        }
    }
}
