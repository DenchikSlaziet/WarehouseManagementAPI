using AutoMapper;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;
using WarehouseManagement.Repositories.Contracts.WriteRepositories;
using WarehouseManagement.Services.Anchors;
using WarehouseManagement.Services.Contracts.Contracts;
using WarehouseManagement.Services.Contracts.Exceptions;
using WarehouseManagement.Services.Contracts.Models;

namespace WarehouseManagement.Services.Services
{
    /// <inheritdoc cref="IProductService"/>
    public class ProductService : IProductService, IServiceAnchor
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceValidator _serviceValidator;

        public ProductService(IProductReadRepository productReadRepository, 
            IProductWriteRepository productWriteRepository, IMapper mapper,
            IUnitOfWork unitOfWork, IServiceValidator serviceValidator)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceValidator = serviceValidator;
        }

        async Task<ProductModel> IProductService.AddAsync(ProductModel model, CancellationToken cancellationToken)
        {
            await _serviceValidator.ValidateAsync(model, cancellationToken);

            var product = _mapper.Map<Product>(model);

            _productWriteRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProductModel>(product);
        }

        async Task IProductService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(id, cancellationToken);

            if (product == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Product>(id);
            }

            _productWriteRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<ProductModel> IProductService.EditAsync(ProductModel model, CancellationToken cancellationToken)
        {
            await _serviceValidator.ValidateAsync(model, cancellationToken);

            var targetProduct = await _productReadRepository.GetByIdAsync(model.Id, cancellationToken);

            if(targetProduct == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Product>(model.Id);
            }

            var times = new { targetProduct.CreatedAt, targetProduct.CreatedBy };
            targetProduct = _mapper.Map<Product>(model);
            targetProduct.CreatedAt = times.CreatedAt;
            targetProduct.CreatedBy = times.CreatedBy;

            _productWriteRepository.Update(targetProduct);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProductModel>(targetProduct);
        }

        async Task<IEnumerable<ProductModel>> IProductService.GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await _productReadRepository.GetAllAsync(cancellationToken);
            return products.Select(x => _mapper.Map<ProductModel>(x));

        }

        async Task<ProductModel?> IProductService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(id, cancellationToken);

            if(product == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Product>(id);
            }

            return _mapper.Map<ProductModel>(product);
        }
    }
}
