using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Test.Extensions
{
    /// <summary>
    /// Расширения для сущностей
    /// </summary>
    public static class TestDataExtensions
    {
        /// <summary>
        /// Автозаполнение свойств
        /// </summary>
        public static void BaseAuditSetParamtrs<TEntity>(this TEntity entity) where TEntity : BaseAuditEntity
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.CreatedBy = $"CreatedBy{Guid.NewGuid():N}";
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}";
        }
    }
}
