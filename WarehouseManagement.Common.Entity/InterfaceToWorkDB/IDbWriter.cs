using WarehouseManagement.Common.Entity.EntityInterface;

namespace WarehouseManagement.Common.Entity.InterfaceToWorkDB
{
    /// <summary>
    /// Контекст для записи в БД
    /// </summary>
    public interface IDbWriter
    {
        /// <summary>
        /// Добавить новую запись
        /// </summary>
        void Add<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        /// Изменить новую запись
        /// </summary>
        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        /// Удалить новую запись
        /// </summary>
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

    }
}
