namespace WarehouseManagement.API.Models.Response
{
    /// <summary>
    /// Модель ответа продукта
    /// </summary>
    public class ProductResponse
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
        /// Описание
        /// </summary>
        public string? Description { get; set; }
    }
}
