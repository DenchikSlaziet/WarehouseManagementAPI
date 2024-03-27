namespace WarehouseManagement.Services.Contracts.ModelsRequest
{
    /// <summary>
    /// Модель запроса SKU
    /// </summary>
    public class WarehouseUnitModelRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

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
