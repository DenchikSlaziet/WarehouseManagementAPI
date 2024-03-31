using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Repositories.Anchors;
using WarehouseManagement.Repositories.Contracts.WriteRepositories;

namespace WarehouseManagement.Repositories.WriteRepositories
{
    /// <summary>
    /// Реализация <see cref="IWarehouseWarehouseUnitWriteRepository"/>
    /// </summary>
    public class WarehouseWarehouseUnitWriteRepository : BaseWriteRepository<WarehouseWarehouseUnit>,
        IWarehouseWarehouseUnitWriteRepository,
        IRepositoryAnchor
    {
        public WarehouseWarehouseUnitWriteRepository(IDbWriterContext writerContext) : base(writerContext)
        {
        }
    }
}
