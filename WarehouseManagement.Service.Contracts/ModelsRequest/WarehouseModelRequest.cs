using WarehouseManagement.Services.Contracts.Models;

namespace WarehouseManagement.Services.Contracts.ModelsRequest
{
    public class WarehouseModelRequest
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
        /// Идентификаторы SKU на складе
        /// </summary>
        public IEnumerable<Guid> WarehouseUnitModelIds { get; set; }
    }
}
