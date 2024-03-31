using WarehouseManagement.API.Models.CreateRequest;

namespace WarehouseManagement.API.Models.Request
{
    /// <summary>
    /// Модель запроса изменения склада
    /// </summary>
    public class WarehouseRequest : WarehouseCreateRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
    }
}
