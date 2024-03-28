using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Repositories.Contracts.ReadRepositories
{
    public interface IWarehouseUnitReadRepository
    {
        /// <summary>
        /// Получить все <see cref="WarehouseUnit"/>
        /// </summary>
        Task<IReadOnlyCollection<WarehouseUnit>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="WarehouseUnit"/> по Id
        /// </summary>
        Task<WarehouseUnit?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все <see cref="WarehouseUnit"/> по идентификаторам
        /// </summary>
        Task<Dictionary<Guid, WarehouseUnit>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);

        /// <summary>
        /// Проверить если <see cref="WarehouseUnit"/> в БД
        /// </summary>
        Task<bool> IsNotNullAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все <see cref="Warehouse"/> по идентификатору <see cref="WarehouseUnit"/>
        /// </summary>
        Task<IReadOnlyCollection<Warehouse>> GetWarehouseByWarehouseUnitId(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все <see cref="WarehouseWarehouseUnit"/> по идентификатору <see cref="WarehouseUnit"/>
        /// </summary>
        Task<IReadOnlyCollection<WarehouseWarehouseUnit>> GetDependenceEntityByWarehouseUnitId(Guid id, CancellationToken cancellationToken);
    }
}
