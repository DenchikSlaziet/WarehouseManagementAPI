using WarehouseManagement.API.Models.CreateRequest;

namespace WarehouseManagement.API.Models.Request
{
    /// <summary>
    /// Модель запроса изменения продукта
    /// </summary>
    public class ProductRequest : ProductCreateRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
    }
}
