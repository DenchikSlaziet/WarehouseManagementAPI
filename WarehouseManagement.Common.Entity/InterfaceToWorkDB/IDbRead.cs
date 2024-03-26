using WarehouseManagement.Common.Entity.EntityInterface;

namespace WarehouseManagement.Common.Entity.InterfaceToWorkDB
{
    /// <summary>
    /// Интерфейс чтения БД
    /// </summary>
    public interface IDbRead
    {
        /// <summary>
        /// Предоставляет функциональные возможности для выполнения запросов
        /// </summary> 
        IQueryable<TEntity> Read<TEntity>() where TEntity : class, IEntity;
    }
}
