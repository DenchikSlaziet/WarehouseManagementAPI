using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.General;
using WarehouseManagement.Repositories.Contracts.Anchors;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;

namespace WarehouseManagement.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="IProductReadRepository"/>
    /// </summary>
    public class ProductReadRepository : IProductReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Контекст для работы с БД
        /// </summary>
        private readonly IDbRead reader;

        public ProductReadRepository(IDbRead reader)
        {
            this.reader = reader;
        }

        Task<IReadOnlyCollection<Product>> IProductReadRepository.GetAllAsync(CancellationToken cancellationToken)
            => reader.Read<Product>()
            .NotDeletedAt()
            .OrderBy(x => x.Title)
            .ToReadOnlyCollectionAsync(cancellationToken);

        Task<Product?> IProductReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Product>()
            .ById(id)
            .NotDeletedAt().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        Task<Dictionary<Guid, Product>> IProductReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
            => reader.Read<Product>()
            .NotDeletedAt()
            .ByIds(ids)
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        Task<bool> IProductReadRepository.IsNotNullAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Product>()
            .NotDeletedAt()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
