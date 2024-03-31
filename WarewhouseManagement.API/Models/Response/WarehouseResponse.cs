namespace WarehouseManagement.API.Models.Response
{
    /// <summary>
    /// Модель ответа склада
    /// </summary>
    public class WarehouseResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// SKU на складе
        /// </summary>
        public IEnumerable<WarehouseUnitResponse> WarehouseUnitModels { get; set; }
    }
}
