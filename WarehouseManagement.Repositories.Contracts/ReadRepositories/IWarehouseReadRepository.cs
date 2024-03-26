using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Repositories.Contracts.ReadRepositories
{
    /// <summary>
    /// Репозиторий чтения <see cref="Warehouse"/>
    /// </summary>
    public interface IWarehouseReadRepository
    {
        /// <summary>
        /// Получить все <see cref="Warehouse"/>
        /// </summary>
        Task<IReadOnlyCollection<Warehouse>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Warehouse"/> по Id
        /// </summary>
        Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все <see cref="Warehouse"/> по идентификаторам
        /// </summary>
        Task<Dictionary<Guid, Warehouse>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);

        /// <summary>
        /// Проверить если <see cref="Warehouse"/> в БД
        /// </summary>
        Task<bool> IsNotNullAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все <see cref="Warehouse"/> по идентификатору <see cref="WarehouseUnit"/>
        /// </summary>
        Task<IReadOnlyCollection<Warehouse>> GetByWarehouseUnitId(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все <see cref="WarehouseWarehouseUnit"/> по идентификатору <see cref="Warehouse"/>
        /// </summary>
        Task<IReadOnlyCollection<WarehouseWarehouseUnit>> GetDependenceEntityByWarehouseId(Guid id, CancellationToken cancellationToken);
    }
}
