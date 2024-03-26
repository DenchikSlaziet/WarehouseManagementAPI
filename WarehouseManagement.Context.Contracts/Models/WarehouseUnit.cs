namespace WarehouseManagement.Context.Contracts.Models
{
    /// <summary>
    /// Сущность единицы складского учета
    /// </summary>
    public class WarehouseUnit : BaseAuditEntity
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        /// <summary>
        /// Кол-во товара
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price { get; set; }

        public ICollection<WarehouseWarehouseUnit> WarehouseWarehouseUnits { get; set; }
    }
}
