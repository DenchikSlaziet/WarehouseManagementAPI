using WarehouseManagement.Common.Entity.EntityInterface;

namespace WarehouseManagement.Services.Contracts.Exceptions
{
    public class WarehouseManagmentEntityNotFoundException<TEntity> : WarehouseManagmentNotFoundException where TEntity : class, IEntity
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="WarehouseManagmentEntityNotFoundException{TEntity}"/>
        /// </summary>
        public WarehouseManagmentEntityNotFoundException(Guid id)
            : base($"Сущность {typeof(TEntity)} c id = {id} не найдена.")
        {
        }
    }
}
