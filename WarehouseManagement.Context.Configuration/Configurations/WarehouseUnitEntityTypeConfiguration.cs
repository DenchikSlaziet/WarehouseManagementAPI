using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Context.Configuration.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="WarehouseUnit"/>
    /// </summary>
    internal class WarehouseUnitEntityTypeConfiguration : IEntityTypeConfiguration<WarehouseUnit>
    {
        void IEntityTypeConfiguration<WarehouseUnit>.Configure(EntityTypeBuilder<WarehouseUnit> builder)
        {
            builder.ToTable("WarehouseUnits");
            builder.PropertyAuditConfiguration();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Count).IsRequired();
            builder.Property(x => x.Price).IsRequired();
        }
    }
}
