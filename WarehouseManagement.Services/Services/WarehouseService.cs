using AutoMapper;
using System.Threading;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;
using WarehouseManagement.Repositories.Contracts.WriteRepositories;
using WarehouseManagement.Services.Contracts.Anchors;
using WarehouseManagement.Services.Contracts.Contracts;
using WarehouseManagement.Services.Contracts.Exceptions;
using WarehouseManagement.Services.Contracts.Models;
using WarehouseManagement.Services.Contracts.ModelsRequest;

namespace WarehouseManagement.Services.Services
{
    public class WarehouseService : IWarehouseService, IServiceAnchor
    {
        private readonly IWarehouseReadRepository warehouseReadRepository;
        private readonly IProductReadRepository productReadRepository;
        private readonly IWarehouseWriteRepository warehouseWriteRepository;
        private readonly IWarehouseWarehouseUnitWriteRepository warehouseWarehouseUnitWriteRepository;
        private readonly IMapper mapper;
        private readonly IServiceValidator serviceValidator;
        private readonly IUnitOfWork unitOfWork;

        public WarehouseService(IWarehouseReadRepository warehouseReadRepository, 
            IWarehouseWriteRepository warehouseWriteRepository, IMapper mapper,
            IServiceValidator serviceValidator, IUnitOfWork unitOfWork,
            IWarehouseWarehouseUnitWriteRepository warehouseWarehouseUnitWriteRepository,
            IProductReadRepository productReadRepository)
        {
            this.warehouseReadRepository = warehouseReadRepository;
            this.warehouseWriteRepository = warehouseWriteRepository;
            this.mapper = mapper;
            this.serviceValidator = serviceValidator;
            this.unitOfWork = unitOfWork;
            this.warehouseWarehouseUnitWriteRepository = warehouseWarehouseUnitWriteRepository;
            this.productReadRepository = productReadRepository;
        }

        async Task<WarehouseModel> IWarehouseService.AddAsync(WarehouseModelRequest modelRequest, CancellationToken cancellationToken)
        {           
            await serviceValidator.ValidateAsync(modelRequest, cancellationToken);

            modelRequest.Id = Guid.NewGuid();

            var warehouse = mapper.Map<Warehouse>(modelRequest);

            warehouseWriteRepository.Add(warehouse);

            foreach (var id in modelRequest.WarehouseUnitModelIds)
            {
                var warehouseWarehouseUnit = new WarehouseWarehouseUnit
                {
                    WarehouseId = warehouse.Id,
                    WarehouseUnitId = id
                };

                warehouseWarehouseUnitWriteRepository.Add(warehouseWarehouseUnit);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var warehouseModel = await GetWarehouseModelInWarehouseAsync(warehouse, cancellationToken);

            return warehouseModel;
        }
        
        async Task IWarehouseService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var warehouse = await warehouseReadRepository.GetByIdAsync(id, cancellationToken);

            if (warehouse == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Warehouse>(id);
            }

            warehouseWriteRepository.Delete(warehouse);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<WarehouseModel> IWarehouseService.EditAsync(WarehouseModelRequest modelRequest, CancellationToken cancellationToken)
        {
            var targetWarehouse = await warehouseReadRepository.GetByIdAsync(modelRequest.Id, cancellationToken);

            if(targetWarehouse == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Warehouse>(modelRequest.Id);
            }

            await serviceValidator.ValidateAsync(modelRequest, cancellationToken);

            targetWarehouse = mapper.Map<Warehouse>(modelRequest);

            warehouseWriteRepository.Update(targetWarehouse);

            var oldWarehouseWarehouseUnits = await warehouseReadRepository
                .GetDependenceEntityByWarehouseId(targetWarehouse.Id, cancellationToken);
            var newWarehouseWarehouseUnits = modelRequest.WarehouseUnitModelIds.
                Select(x => new WarehouseWarehouseUnit
                {
                WarehouseId = targetWarehouse.Id,
                WarehouseUnitId = x
                });

            oldWarehouseWarehouseUnits.Except(newWarehouseWarehouseUnits).ToList()
                .ForEach(x => warehouseWarehouseUnitWriteRepository.Delete(x));
            newWarehouseWarehouseUnits.Except(oldWarehouseWarehouseUnits).ToList()
                .ForEach(x => warehouseWarehouseUnitWriteRepository.Add(x));
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var warehouseModel = await GetWarehouseModelInWarehouseAsync(targetWarehouse, cancellationToken);

            return warehouseModel;
        }

        async Task<IEnumerable<WarehouseModel>> IWarehouseService.GetAllAsync(CancellationToken cancellationToken)
        {
            var warehouses = await warehouseReadRepository.GetAllAsync(cancellationToken);
            var warehouseModels = new List<WarehouseModel>(warehouses.Count);

            foreach (var warehouse in warehouses)
            {
                var warehouseModel = await GetWarehouseModelInWarehouseAsync(warehouse, cancellationToken);

                warehouseModels.Add(warehouseModel);
            }

            return warehouseModels;
        }
        
        async Task<WarehouseModel?> IWarehouseService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var warehouse = await warehouseReadRepository.GetByIdAsync(id, cancellationToken);

            if(warehouse == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Warehouse>(id);
            }

            var warehouseModel = await GetWarehouseModelInWarehouseAsync(warehouse, cancellationToken);

            return warehouseModel;
        }

        async private Task<WarehouseModel> GetWarehouseModelInWarehouseAsync(Warehouse warehouse, CancellationToken cancellationToken)
        {
            var warehouseModel = mapper.Map<WarehouseModel>(warehouse);
            var warehouseUnits = await warehouseReadRepository.GetWarehouseUnitByWarehouseId(warehouse.Id, cancellationToken);
            var productsIds = await productReadRepository.GetByIdsAsync(warehouseUnits
                .Select(x => x.ProductId).Distinct(), cancellationToken);
            var warehouseUnitModels = new List<WarehouseUnitModel>(warehouseUnits.Count);

            foreach (var warehouseUnit in warehouseUnits)
            {
                if (productsIds.TryGetValue(warehouseUnit.ProductId, out var product))
                {
                    var warehouseUnitModel = mapper.Map<WarehouseUnitModel>(warehouseUnit);
                    warehouseUnitModel.Product = mapper.Map<ProductModel>(product);

                    warehouseUnitModels.Add(warehouseUnitModel);
                }
            }

            warehouseModel.WarehouseUnitModels = warehouseUnitModels;

            return warehouseModel;
        }
    }
}
