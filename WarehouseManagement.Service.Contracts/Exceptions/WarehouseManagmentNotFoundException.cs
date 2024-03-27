namespace WarehouseManagement.Services.Contracts.Exceptions
{
    public class WarehouseManagmentNotFoundException : WarehouseManagmentException
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="WarehouseManagmentNotFoundException"/> с указанием
        /// сообщения об ошибке
        /// </summary>
        public WarehouseManagmentNotFoundException(string message)
            : base(message)
        { }
    }
}
