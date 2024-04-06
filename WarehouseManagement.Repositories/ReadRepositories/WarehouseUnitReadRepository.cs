using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.General;
using WarehouseManagement.Repositories.Anchors;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;

namespace WarehouseManagement.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <<see cref="IWarehouseUnitReadRepository"/>
    /// </summary>
    public class WarehouseUnitReadRepository : IWarehouseUnitReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Контекст для работы с бд
        /// </summary>
        private readonly IDbRead _reader;

        public WarehouseUnitReadRepository(IDbRead reader)
        {
            _reader = reader;
        }

        Task<IReadOnlyCollection<WarehouseUnit>> IWarehouseUnitReadRepository.GetAllAsync(CancellationToken cancellationToken)
            => _reader.Read<WarehouseUnit>()
            .NotDeletedAt()
            .OrderBy(x => x.Price)
            .ToReadOnlyCollectionAsync(cancellationToken);

        Task<WarehouseUnit?> IWarehouseUnitReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => _reader.Read<WarehouseUnit>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        Task<Dictionary<Guid, WarehouseUnit>> IWarehouseUnitReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
            => _reader.Read<WarehouseUnit>()
            .NotDeletedAt()
            .ByIds(ids)
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        Task<IReadOnlyCollection<Warehouse>> IWarehouseUnitReadRepository.GetWarehouseByWarehouseUnitId(Guid id, CancellationToken cancellationToken)
            => _reader.Read<Warehouse>()
            .NotDeletedAt()
            .Where(x => x.WarehouseWarehouseUnits.Any(x => x.WarehouseUnitId == id))
            .ToReadOnlyCollectionAsync(cancellationToken);

        Task<IReadOnlyCollection<WarehouseWarehouseUnit>> IWarehouseUnitReadRepository.GetDependenceEntityByWarehouseUnitId(Guid id, CancellationToken cancellationToken)
            => _reader.Read<WarehouseWarehouseUnit>()
            .Where(x => x.WarehouseUnitId == id)
            .ToReadOnlyCollectionAsync(cancellationToken);

        Task<bool> IWarehouseUnitReadRepository.IsNotNullAsync(Guid id, CancellationToken cancellationToken)
            => _reader.Read<WarehouseUnit>()
            .NotDeletedAt()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
