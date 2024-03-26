using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Repositories.Contracts.Anchors;
using WarehouseManagement.Repositories.Contracts.WriteRepositories;

namespace WarehouseManagement.Repositories.WriteRepositories
{
    /// <summary>
    /// Реализация <see cref="IWarehouseWriteRepository"/>
    /// </summary>
    public class WarehouseWriteRepository : BaseWriteRepository<Warehouse>,
        IWarehouseWriteRepository, 
        IRepositoryAnchor
    {
        public WarehouseWriteRepository(IDbWriterContext writerContext) : base(writerContext)
        {
        }
    }
}
