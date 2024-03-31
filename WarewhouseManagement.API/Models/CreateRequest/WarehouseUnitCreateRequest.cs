namespace WarehouseManagement.API.Models.CreateRequest
{
    /// <summary>
    /// Модель запроса создания SKU
    /// </summary>
    public class WarehouseUnitCreateRequest
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }

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
