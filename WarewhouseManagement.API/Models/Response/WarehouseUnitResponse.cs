namespace WarehouseManagement.API.Models.Response
{
    /// <summary>
    /// Модель ответа SKU
    /// </summary>
    public class WarehouseUnitResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Сущность товара
        /// </summary>
        public ProductResponse Product { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// Кол-во товара
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price { get; set; }
    }
}
