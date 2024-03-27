using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WarehouseManagement.Context
{
    /// <summary>
    /// Фабрика для создания контекста в DesignTime (Миграции)
    /// </summary>
    public class SampleContextFactory : IDesignTimeDbContextFactory<WarehouseManagementContext>
    {
        public WarehouseManagementContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<WarehouseManagementContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new WarehouseManagementContext(options);
        }
    }
}
