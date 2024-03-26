using WarehouseManagement.Common.Entity.EntityInterface;

namespace WarehouseManagement.Context.Contracts.Models
{
    /// <summary>
    /// Промежуточная таблица для склада и единиц складского учета
    /// </summary>
    public class WarehouseWarehouseUnit : IEntity
    {
        /// <summary>
        /// Идентификатор склада
        /// </summary>
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        /// <summary>
        /// Идентификатор единицы складского учета
        /// </summary>
        public Guid WarehouseUnitId { get; set; }
        public WarehouseUnit WarehouseUnit { get; set; }
    }
}
