namespace WarehouseManagement.Context.Contracts.Models
{
    /// <summary>
    /// Сущность продукта
    /// </summary>
    public class Product : BaseAuditEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }

        public ICollection<WarehouseUnit> WarehouseUnits { get; set; }
    }
}
