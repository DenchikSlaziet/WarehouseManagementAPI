namespace WarehouseManagement.API.Models.CreateRequest
{
    /// <summary>
    /// Модель запроса создания продукта
    /// </summary>
    public class ProductCreateRequest
    {
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
