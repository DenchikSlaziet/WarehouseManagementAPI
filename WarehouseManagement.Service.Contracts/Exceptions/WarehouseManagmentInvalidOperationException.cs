namespace WarehouseManagement.Services.Contracts.Exceptions
{
    public class WarehouseManagmentInvalidOperationException : WarehouseManagmentException
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="WarehouseManagmentInvalidOperationException"/>
        /// с указанием сообщения об ошибке
        /// </summary>
        public WarehouseManagmentInvalidOperationException(string message)
            : base(message)
        {

        }
    }
}
