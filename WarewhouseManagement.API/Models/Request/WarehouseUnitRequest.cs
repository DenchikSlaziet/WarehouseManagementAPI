using WarehouseManagement.API.Models.CreateRequest;

namespace WarehouseManagement.API.Models.Request
{
    /// <summary>
    /// Модель запроса изменения SKU
    /// </summary>
    public class WarehouseUnitRequest : WarehouseUnitCreateRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
    }
}
