using WarehouseManagement.General;

namespace WarehouseManagement.Services.Contracts.Exceptions
{
    public class WarehouseManagmentValidationException : WarehouseManagmentException
    {
        /// <summary>
        /// Ошибки
        /// </summary>
        public IEnumerable<InvalidateItemModel> Errors { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="WarehouseManagmentValidationException"/>
        /// </summary>
        public WarehouseManagmentValidationException(IEnumerable<InvalidateItemModel> errors)
        {
            Errors = errors;
        }
    }
}
