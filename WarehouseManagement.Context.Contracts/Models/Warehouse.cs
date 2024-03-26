namespace WarehouseManagement.Context.Contracts.Models
{
    /// <summary>
    /// Сущность склада
    /// </summary>
    public class Warehouse : BaseAuditEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; } = string.Empty;

        public ICollection<WarehouseWarehouseUnit> WarehouseWarehouseUnits { get; set; }
    }
}
