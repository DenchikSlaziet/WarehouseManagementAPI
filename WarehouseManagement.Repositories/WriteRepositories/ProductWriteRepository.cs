using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Repositories.Anchors;
using WarehouseManagement.Repositories.Contracts.WriteRepositories;

namespace WarehouseManagement.Repositories.WriteRepositories
{
    /// <summary>
    /// Реализация <see cref="IProductWriteRepository"/>
    /// </summary>
    public class ProductWriteRepository : BaseWriteRepository<Product>, 
        IProductWriteRepository, 
        IRepositoryAnchor
    {
        public ProductWriteRepository(IDbWriterContext writerContext) : base(writerContext)
        {
        }
    }
}
