namespace WarehouseManagement.Services.Contracts.Exceptions
{
    public abstract class WarehouseManagmentException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="WarehouseManagmentException"/> без параметров
        /// </summary>
        protected WarehouseManagmentException() { }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="WarehouseManagmentException"/> с указанием
        /// сообщения об ошибке
        /// </summary>
        protected WarehouseManagmentException(string message)
            : base(message) { }
    }
}
