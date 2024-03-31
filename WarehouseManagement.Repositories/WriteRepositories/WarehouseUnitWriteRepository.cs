using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Repositories.Anchors;
using WarehouseManagement.Repositories.Contracts.WriteRepositories;

namespace WarehouseManagement.Repositories.WriteRepositories
{
    /// <summary>
    /// Реализация <see cref="IWarehouseUnitWriteRepository"/>
    /// </summary>
    public class WarehouseUnitWriteRepository : BaseWriteRepository<WarehouseUnit>, 
        IWarehouseUnitWriteRepository, 
        IRepositoryAnchor
    {
        public WarehouseUnitWriteRepository(IDbWriterContext writerContext) : base(writerContext)
        {
        }
    }
}
