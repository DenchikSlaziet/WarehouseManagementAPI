using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.General;
using WarehouseManagement.Repositories.Contracts.Anchors;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;

namespace WarehouseManagement.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="IWarehouseReadRepository"/>
    /// </summary>
    public class WarehouseReadRepository : IWarehouseReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Контекст для работы с БД
        /// </summary>
        private readonly IDbRead reader;

        public WarehouseReadRepository(IDbRead reader)
        {
            this.reader = reader;
        }

        Task<IReadOnlyCollection<Warehouse>> IWarehouseReadRepository.GetAllAsync(CancellationToken cancellationToken)
            => reader.Read<Warehouse>()
            .NotDeletedAt()
            .OrderBy(x => x.Title)
            .ThenBy(x => x.Address)
            .ToReadOnlyCollectionAsync(cancellationToken);

        Task<Warehouse?> IWarehouseReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Warehouse>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        Task<Dictionary<Guid, Warehouse>> IWarehouseReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
            => reader.Read<Warehouse>()
            .NotDeletedAt()
            .ByIds(ids)
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        Task<IReadOnlyCollection<Warehouse>> IWarehouseReadRepository.GetByWarehouseUnitId(Guid id, CancellationToken cancellationToken)
            => reader.Read<Warehouse>()
            .NotDeletedAt()
            .Where(x => x.WarehouseWarehouseUnits.Any(x => x.WarehouseUnitId == id))
            .ToReadOnlyCollectionAsync(cancellationToken);

        Task<IReadOnlyCollection<WarehouseWarehouseUnit>> IWarehouseReadRepository.GetDependenceEntityByWarehouseId(Guid id, CancellationToken cancellationToken)
            => reader.Read<WarehouseWarehouseUnit>()            
            .Where(x => x.WarehouseId == id)
            .ToReadOnlyCollectionAsync(cancellationToken);

        Task<bool> IWarehouseReadRepository.IsNotNullAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Warehouse>()
            .NotDeletedAt()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
