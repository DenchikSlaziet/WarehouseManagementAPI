namespace WarehouseManagement.Services.Contracts.Contracts
{
    /// <summary>
    /// Сервис валидации
    /// </summary>
    public interface IServiceValidatorService
    {
        /// <summary>
        /// Валидирует модель
        /// </summary>
        Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken)
            where TModel : class;
    }
}
