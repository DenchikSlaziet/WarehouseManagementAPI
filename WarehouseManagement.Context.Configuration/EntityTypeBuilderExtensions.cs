using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Context.Configuration
{
    /// <summary>
    /// Методы расширения для <see cref="EntityTypeBuilder"/>
    /// </summary>
    internal static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Задаёт конфигурацию свойств аудита для сущности <inheritdoc cref="BaseAuditEntity"/>
        /// </summary>
        public static void PropertyAuditConfiguration<T>(this EntityTypeBuilder<T> builder)
            where T : BaseAuditEntity
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(200);
            builder.Property(x => x.UpdatedAt).IsRequired();
            builder.Property(x => x.UpdatedBy).IsRequired().HasMaxLength(200);
        }
    }
}