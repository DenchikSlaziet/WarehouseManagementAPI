namespace WarehouseManagement.Services.Contracts.Models
{
    /// <summary>
    /// Модель SKU
    /// </summary>
    public class WarehouseUnitModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public ProductModel Product { get; set; }

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
