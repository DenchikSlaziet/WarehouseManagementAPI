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
        private readonly IWarehouseUnitReadRepository _warehouseUnitReadRepository;
        private readonly IWarehouseUnitWriteRepository _warehouseUnitWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceValidator _serviceValidator;

        public WarehouseUnitService(IWarehouseUnitReadRepository warehouseUnitReadRepository,
            IWarehouseUnitWriteRepository warehouseUnitWriteRepository, IProductReadRepository productReadRepository,
            IMapper mapper, IUnitOfWork unitOfWork, IServiceValidator serviceValidator)
        {
            _warehouseUnitReadRepository = warehouseUnitReadRepository;
            _warehouseUnitWriteRepository = warehouseUnitWriteRepository;
            _productReadRepository = productReadRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceValidator = serviceValidator;
        }

        async Task<WarehouseUnitModel> IWarehouseUnitService.AddAsync(WarehouseUnitModelRequest modelRequest, CancellationToken cancellationToken)
        {
            await _serviceValidator.ValidateAsync(modelRequest, cancellationToken);

            modelRequest.Id = Guid.NewGuid();
            var warehouseUnit = _mapper.Map<WarehouseUnit>(modelRequest);

            _warehouseUnitWriteRepository.Add(warehouseUnit);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var product = await _productReadRepository.GetByIdAsync(warehouseUnit.ProductId, cancellationToken);
            var warehouseUnitModel = _mapper.Map<WarehouseUnitModel>(warehouseUnit);
            warehouseUnitModel.Product = _mapper.Map<ProductModel>(product);

            return warehouseUnitModel;
        }

        async Task IWarehouseUnitService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var warehouseUnit = await _warehouseUnitReadRepository.GetByIdAsync(id, cancellationToken);

            if (warehouseUnit == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<WarehouseUnit>(id);
            }

            _warehouseUnitWriteRepository.Delete(warehouseUnit);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<WarehouseUnitModel> IWarehouseUnitService.EditAsync(WarehouseUnitModelRequest modelRequest, CancellationToken cancellationToken)
        {
            await _serviceValidator.ValidateAsync(modelRequest, cancellationToken);

            var targetWarehouseUnit = await _warehouseUnitReadRepository.GetByIdAsync(modelRequest.Id, cancellationToken);

            if(targetWarehouseUnit == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<WarehouseUnit>(modelRequest.Id);
            }

            var times = new { targetWarehouseUnit.CreatedAt, targetWarehouseUnit.CreatedBy };
            targetWarehouseUnit = _mapper.Map<WarehouseUnit>(modelRequest);
            targetWarehouseUnit.CreatedAt = times.CreatedAt;
            targetWarehouseUnit.CreatedBy = times.CreatedBy;

            _warehouseUnitWriteRepository.Update(targetWarehouseUnit);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var product = await _productReadRepository.GetByIdAsync(targetWarehouseUnit.ProductId, cancellationToken);        
            var warehouseUnitModel = _mapper.Map<WarehouseUnitModel>(targetWarehouseUnit);
            warehouseUnitModel.Product = _mapper.Map<ProductModel>(product);

            return warehouseUnitModel;
        }

        async Task<IEnumerable<WarehouseUnitModel>> IWarehouseUnitService.GetAllAsync(CancellationToken cancellationToken)
        {
            var warehouseUnits = await _warehouseUnitReadRepository.GetAllAsync(cancellationToken);
            var productIds = await _productReadRepository.GetByIdsAsync(warehouseUnits
                .Select(x => x.ProductId).Distinct(), cancellationToken);
            var warehouseUnitModels = new List<WarehouseUnitModel>(warehouseUnits.Count);

            foreach (var warehouseUnit in warehouseUnits)
            {
                if(productIds.TryGetValue(warehouseUnit.ProductId, out var product))
                {
                    var warehouseUnitModel = _mapper.Map<WarehouseUnitModel>(warehouseUnit);
                    warehouseUnitModel.Product = _mapper.Map<ProductModel>(product);

                    warehouseUnitModels.Add(warehouseUnitModel);
                }
            }

            return warehouseUnitModels;
        }

        async Task<WarehouseUnitModel?> IWarehouseUnitService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var warehouseUnit = await _warehouseUnitReadRepository.GetByIdAsync(id, cancellationToken);

            if(warehouseUnit == null) 
            {
                throw new WarehouseManagmentEntityNotFoundException<WarehouseUnit>(id);
            }

            var product = await _productReadRepository.GetByIdAsync(warehouseUnit.ProductId, cancellationToken);

            if(product == null) 
            {
                throw new WarehouseManagmentEntityNotFoundException<Product>(warehouseUnit.ProductId);
            }

            var warehouseUnitModel = _mapper.Map<WarehouseUnitModel>(warehouseUnit);
            warehouseUnitModel.Product = _mapper.Map<ProductModel>(product);

            return warehouseUnitModel;
        }
    }
}
