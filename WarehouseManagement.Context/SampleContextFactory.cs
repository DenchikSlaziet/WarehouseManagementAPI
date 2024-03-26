using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WarehouseManagement.Context
{
    /// <summary>
    /// Фабрика для создания контекста в DesignTime (Миграции)
    /// </summary>
    public class SampleContextFactory : IDesignTimeDbContextFactory<WarehouseManagmentContext>
    {
        public WarehouseManagmentContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<WarehouseManagmentContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new WarehouseManagmentContext(options);
        }
    }
}
