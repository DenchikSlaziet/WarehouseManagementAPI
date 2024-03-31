namespace WarehouseManagement.API.Models.CreateRequest
{
    /// <summary>
    /// Модель запроса создания склада
    /// </summary>
    public class WarehouseCreateRequest
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Идентификаторы SKU на складе
        /// </summary>
        public IEnumerable<Guid> WarehouseUnitModelIds { get; set; }
    }
}
