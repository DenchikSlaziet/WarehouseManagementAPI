using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Context.Configuration.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="Warehouse"/>
    /// </summary>
    internal class WarehouseEntityTypeConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        void IEntityTypeConfiguration<Warehouse>.Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");
            builder.PropertyAuditConfiguration();
            builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(100).IsRequired();
            builder.HasIndex(x => x.Address).IsUnique()
                .HasDatabaseName($"{nameof(Warehouse)}_{nameof(Warehouse.Address)}")
                .HasFilter($"{nameof(Warehouse.DeletedAt)} is null");
        }
    }
}
