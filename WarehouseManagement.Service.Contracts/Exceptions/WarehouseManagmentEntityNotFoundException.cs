using WarehouseManagement.Common.Entity.EntityInterface;

namespace WarehouseManagement.Services.Contracts.Exceptions
{
    public class WarehouseManagmentEntityNotFoundException<TEntity> : WarehouseManagmentNotFoundException where TEntity : class, IEntity
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="WarehouseManagmentEntityNotFoundException"/> с указанием
        /// сообщения об ошибке
        /// </summary>
        public WarehouseManagmentEntityNotFoundException(string message) : base(message)
        {
        }
    }
}
