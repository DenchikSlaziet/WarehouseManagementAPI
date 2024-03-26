using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Context.Configuration.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="Product"/>
    /// </summary>
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        void IEntityTypeConfiguration<Product>.Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.PropertyAuditConfiguration();
            builder.Property(x => x.Title).HasMaxLength(40).IsRequired();
            builder.HasIndex(x => x.Title).IsUnique()
                .HasDatabaseName($"{nameof(Product)}_{nameof(Product.Title)}")
                .HasFilter($"{nameof(Product.DeletedAt)} is null");
            builder.Property(x => x.Description).HasMaxLength(100);
            builder.HasMany(x => x.WarehouseUnits).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
        }
    }
}
