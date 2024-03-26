using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Repositories.Contracts.ReadRepositories
{
    /// <summary>
    /// Репозиторий чтения <see cref="Product"/>
    /// </summary>
    public interface IProductReadRepository
    {
        /// <summary>
        /// Получить все <see cref="Product"/>
        /// </summary>
        Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Product"/> по Id
        /// </summary>
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все <see cref="Product"/> по идентификаторам
        /// </summary>
        Task<Dictionary<Guid,Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);

        /// <summary>
        /// Проверить если <see cref="Product"/> в БД
        /// </summary>
        Task<bool> IsNotNullAsync(Guid id, CancellationToken cancellationToken);
    }
}
