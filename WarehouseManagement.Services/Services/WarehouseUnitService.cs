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
    /// <inheritdoc cref="IWarehouseUnitService"/>
    public class WarehouseUnitService : IWarehouseUnitService, IServiceAnchor
    {
        private readonly IWarehouseUnitReadRepository warehouseUnitReadRepository;
        private readonly IWarehouseUnitWriteRepository warehouseUnitWriteRepository;
        private readonly IProductReadRepository productReadRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IServiceValidator serviceValidator;

        public WarehouseUnitService(IWarehouseUnitReadRepository warehouseUnitReadRepository,
            IWarehouseUnitWriteRepository warehouseUnitWriteRepository, IProductReadRepository productReadRepository,
            IMapper mapper, IUnitOfWork unitOfWork, IServiceValidator serviceValidator)
        {
            this.warehouseUnitReadRepository = warehouseUnitReadRepository;
            this.warehouseUnitWriteRepository = warehouseUnitWriteRepository;
            this.productReadRepository = productReadRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.serviceValidator = serviceValidator;
        }

        async Task<WarehouseUnitModel> IWarehouseUnitService.AddAsync(WarehouseUnitModelRequest modelRequest, CancellationToken cancellationToken)
        {
            await serviceValidator.ValidateAsync(modelRequest, cancellationToken);

            modelRequest.Id = Guid.NewGuid();
            var warehouseUnit = mapper.Map<WarehouseUnit>(modelRequest);

            warehouseUnitWriteRepository.Add(warehouseUnit);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var product = await productReadRepository.GetByIdAsync(warehouseUnit.ProductId, cancellationToken);
            var warehouseUnitModel = mapper.Map<WarehouseUnitModel>(warehouseUnit);
            warehouseUnitModel.Product = mapper.Map<ProductModel>(product);

            return warehouseUnitModel;
        }

        async Task IWarehouseUnitService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var warehouseUnit = await warehouseUnitReadRepository.GetByIdAsync(id, cancellationToken);

            if (warehouseUnit == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<WarehouseUnit>(id);
            }

            warehouseUnitWriteRepository.Delete(warehouseUnit);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<WarehouseUnitModel> IWarehouseUnitService.EditAsync(WarehouseUnitModelRequest modelRequest, CancellationToken cancellationToken)
        {
            await serviceValidator.ValidateAsync(modelRequest, cancellationToken);

            var targetWarehouseUnit = await warehouseUnitReadRepository.GetByIdAsync(modelRequest.Id, cancellationToken);

            if(targetWarehouseUnit == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<WarehouseUnit>(modelRequest.Id);
            }

            var times = new { targetWarehouseUnit.CreatedAt, targetWarehouseUnit.CreatedBy };
            targetWarehouseUnit = mapper.Map<WarehouseUnit>(modelRequest);
            targetWarehouseUnit.CreatedAt = times.CreatedAt;
            targetWarehouseUnit.CreatedBy = times.CreatedBy;

            warehouseUnitWriteRepository.Update(targetWarehouseUnit);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var product = await productReadRepository.GetByIdAsync(targetWarehouseUnit.ProductId, cancellationToken);        
            var warehouseUnitModel = mapper.Map<WarehouseUnitModel>(targetWarehouseUnit);
            warehouseUnitModel.Product = mapper.Map<ProductModel>(product);

            return warehouseUnitModel;
        }

        async Task<IEnumerable<WarehouseUnitModel>> IWarehouseUnitService.GetAllAsync(CancellationToken cancellationToken)
        {
            var warehouseUnits = await warehouseUnitReadRepository.GetAllAsync(cancellationToken);
            var productIds = await productReadRepository.GetByIdsAsync(warehouseUnits
                .Select(x => x.ProductId).Distinct(), cancellationToken);
            var warehouseUnitModels = new List<WarehouseUnitModel>(warehouseUnits.Count);

            foreach (var warehouseUnit in warehouseUnits)
            {
                if(productIds.TryGetValue(warehouseUnit.ProductId, out var product))
                {
                    var warehouseUnitModel = mapper.Map<WarehouseUnitModel>(warehouseUnit);
                    warehouseUnitModel.Product = mapper.Map<ProductModel>(product);

                    warehouseUnitModels.Add(warehouseUnitModel);
                }
            }

            return warehouseUnitModels;
        }

        async Task<WarehouseUnitModel?> IWarehouseUnitService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var warehouseUnit = await warehouseUnitReadRepository.GetByIdAsync(id, cancellationToken);

            if(warehouseUnit == null) 
            {
                throw new WarehouseManagmentEntityNotFoundException<WarehouseUnit>(id);
            }

            var product = await productReadRepository.GetByIdAsync(warehouseUnit.ProductId, cancellationToken);

            if(product == null) 
            {
                throw new WarehouseManagmentEntityNotFoundException<Product>(warehouseUnit.ProductId);
            }

            var warehouseUnitModel = mapper.Map<WarehouseUnitModel>(warehouseUnit);
            warehouseUnitModel.Product = mapper.Map<ProductModel>(product);

            return warehouseUnitModel;
        }
    }
}
