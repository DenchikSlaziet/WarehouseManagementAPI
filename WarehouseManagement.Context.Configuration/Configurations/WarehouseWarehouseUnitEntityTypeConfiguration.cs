using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Context.Configuration.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="WarehouseWarehouseUnit"/>
    /// </summary>
    internal class WarehouseWarehouseUnitEntityTypeConfiguration : IEntityTypeConfiguration<WarehouseWarehouseUnit>
    {
        void IEntityTypeConfiguration<WarehouseWarehouseUnit>.Configure(EntityTypeBuilder<WarehouseWarehouseUnit> builder)
        {
            builder.ToTable("WarehouseWarehouseUnits");
            builder.HasKey(x => new { x.WarehouseId, x.WarehouseUnitId });
            builder.HasOne(x => x.Warehouse).WithMany(x => x.WarehouseWarehouseUnits).HasForeignKey(x => x.WarehouseId);
            builder.HasOne(x => x.WarehouseUnit).WithMany(x => x.WarehouseWarehouseUnits).HasForeignKey(x => x.WarehouseUnitId);
        }
    }
}
