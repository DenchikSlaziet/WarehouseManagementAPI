using AutoMapper;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;
using WarehouseManagement.Repositories.Contracts.WriteRepositories;
using WarehouseManagement.Services.Anchors;
using WarehouseManagement.Services.Contracts.Contracts;
using WarehouseManagement.Services.Contracts.Exceptions;
using WarehouseManagement.Services.Contracts.Models;
using WarehouseManagement.Services.Contracts.ModelsRequest;

namespace WarehouseManagement.Services.Services
{
    /// <inheritdoc cref="IWarehouseService"/>
    public class WarehouseService : IWarehouseService, IServiceAnchor
    {
        private readonly IWarehouseReadRepository _warehouseReadRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWarehouseWriteRepository _warehouseWriteRepository;
        private readonly IWarehouseWarehouseUnitWriteRepository _warehouseWarehouseUnitWriteRepository;
        private readonly IMapper _mapper;
        private readonly IServiceValidator _serviceValidator;
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseService(IWarehouseReadRepository warehouseReadRepository, 
            IWarehouseWriteRepository warehouseWriteRepository, IMapper mapper,
            IServiceValidator serviceValidator, IUnitOfWork unitOfWork,
            IWarehouseWarehouseUnitWriteRepository warehouseWarehouseUnitWriteRepository,
            IProductReadRepository productReadRepository)
        {
            _warehouseReadRepository = warehouseReadRepository;
            _warehouseWriteRepository = warehouseWriteRepository;
            _mapper = mapper;
            _serviceValidator = serviceValidator;
            _unitOfWork = unitOfWork;
            _warehouseWarehouseUnitWriteRepository = warehouseWarehouseUnitWriteRepository;
            _productReadRepository = productReadRepository;
        }

        async Task<WarehouseModel> IWarehouseService.AddAsync(WarehouseModelRequest modelRequest, CancellationToken cancellationToken)
        {           
            await _serviceValidator.ValidateAsync(modelRequest, cancellationToken);

            modelRequest.Id = Guid.NewGuid();

            var warehouse = _mapper.Map<Warehouse>(modelRequest);

            _warehouseWriteRepository.Add(warehouse);

            foreach (var id in modelRequest.WarehouseUnitModelIds)
            {
                var warehouseWarehouseUnit = new WarehouseWarehouseUnit
                {
                    WarehouseId = warehouse.Id,
                    WarehouseUnitId = id
                };

                _warehouseWarehouseUnitWriteRepository.Add(warehouseWarehouseUnit);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var warehouseModel = await GetWarehouseModelInWarehouseAsync(warehouse, cancellationToken);

            return warehouseModel;
        }
        
        async Task IWarehouseService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseReadRepository.GetByIdAsync(id, cancellationToken);

            if (warehouse == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Warehouse>(id);
            }

            _warehouseWriteRepository.Delete(warehouse);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<WarehouseModel> IWarehouseService.EditAsync(WarehouseModelRequest modelRequest, CancellationToken cancellationToken)
        {
            var targetWarehouse = await _warehouseReadRepository.GetByIdAsync(modelRequest.Id, cancellationToken);

            if(targetWarehouse == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Warehouse>(modelRequest.Id);
            }

            await _serviceValidator.ValidateAsync(modelRequest, cancellationToken);

            var times = new { targetWarehouse.CreatedAt, targetWarehouse.CreatedBy };
            targetWarehouse = _mapper.Map<Warehouse>(modelRequest);
            targetWarehouse.CreatedBy = times.CreatedBy;
            targetWarehouse.CreatedAt = times.CreatedAt;

            _warehouseWriteRepository.Update(targetWarehouse);

            var oldWarehouseWarehouseUnits = await _warehouseReadRepository
                .GetDependenceEntityByWarehouseId(targetWarehouse.Id, cancellationToken);
            var newWarehouseWarehouseUnits = modelRequest.WarehouseUnitModelIds.
                Select(x => new WarehouseWarehouseUnit
                {
                WarehouseId = targetWarehouse.Id,
                WarehouseUnitId = x
                });

            oldWarehouseWarehouseUnits.Except(newWarehouseWarehouseUnits).ToList()
                .ForEach(x => _warehouseWarehouseUnitWriteRepository.Delete(x));
            newWarehouseWarehouseUnits.Except(oldWarehouseWarehouseUnits).ToList()
                .ForEach(x => _warehouseWarehouseUnitWriteRepository.Add(x));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var warehouseModel = await GetWarehouseModelInWarehouseAsync(targetWarehouse, cancellationToken);

            return warehouseModel;
        }

        async Task<IEnumerable<WarehouseModel>> IWarehouseService.GetAllAsync(CancellationToken cancellationToken)
        {
            var warehouses = await _warehouseReadRepository.GetAllAsync(cancellationToken);
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
            var warehouse = await _warehouseReadRepository.GetByIdAsync(id, cancellationToken);

            if(warehouse == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Warehouse>(id);
            }

            var warehouseModel = await GetWarehouseModelInWarehouseAsync(warehouse, cancellationToken);

            return warehouseModel;
        }

        async private Task<WarehouseModel> GetWarehouseModelInWarehouseAsync(Warehouse warehouse, CancellationToken cancellationToken)
        {
            var warehouseModel = _mapper.Map<WarehouseModel>(warehouse);
            var warehouseUnits = await _warehouseReadRepository.GetWarehouseUnitByWarehouseId(warehouse.Id, cancellationToken);
            var productsIds = await _productReadRepository.GetByIdsAsync(warehouseUnits
                .Select(x => x.ProductId).Distinct(), cancellationToken);
            var warehouseUnitModels = new List<WarehouseUnitModel>(warehouseUnits.Count);

            foreach (var warehouseUnit in warehouseUnits)
            {
                if (productsIds.TryGetValue(warehouseUnit.ProductId, out var product))
                {
                    var warehouseUnitModel = _mapper.Map<WarehouseUnitModel>(warehouseUnit);
                    warehouseUnitModel.Product = _mapper.Map<ProductModel>(product);

                    warehouseUnitModels.Add(warehouseUnitModel);
                }
            }

            warehouseModel.WarehouseUnitModels = warehouseUnitModels;

            return warehouseModel;
        }
    }
}
