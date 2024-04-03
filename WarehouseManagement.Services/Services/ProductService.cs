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
        private readonly IProductReadRepository productReadRepository;
        private readonly IProductWriteRepository productWriteRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IServiceValidator serviceValidator;

        public ProductService(IProductReadRepository productReadRepository, 
            IProductWriteRepository productWriteRepository, IMapper mapper,
            IUnitOfWork unitOfWork, IServiceValidator serviceValidator)
        {
            this.productWriteRepository = productWriteRepository;
            this.productReadRepository = productReadRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.serviceValidator = serviceValidator;
        }

        async Task<ProductModel> IProductService.AddAsync(ProductModel model, CancellationToken cancellationToken)
        {
            await serviceValidator.ValidateAsync(model, cancellationToken);

            var product = mapper.Map<Product>(model);

            productWriteRepository.Add(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ProductModel>(product);
        }

        async Task IProductService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await productReadRepository.GetByIdAsync(id, cancellationToken);

            if (product == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Product>(id);
            }

            productWriteRepository.Delete(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<ProductModel> IProductService.EditAsync(ProductModel model, CancellationToken cancellationToken)
        {
            await serviceValidator.ValidateAsync(model, cancellationToken);

            var targetProduct = await productReadRepository.GetByIdAsync(model.Id, cancellationToken);

            if(targetProduct == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Product>(model.Id);
            }

            var times = new { targetProduct.CreatedAt, targetProduct.CreatedBy };
            targetProduct = mapper.Map<Product>(model);
            targetProduct.CreatedAt = times.CreatedAt;
            targetProduct.CreatedBy = times.CreatedBy;

            productWriteRepository.Update(targetProduct);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ProductModel>(targetProduct);
        }

        async Task<IEnumerable<ProductModel>> IProductService.GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await productReadRepository.GetAllAsync(cancellationToken);
            return products.Select(x => mapper.Map<ProductModel>(x));

        }

        async Task<ProductModel?> IProductService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await productReadRepository.GetByIdAsync(id, cancellationToken);

            if(product == null)
            {
                throw new WarehouseManagmentEntityNotFoundException<Product>(id);
            }

            return mapper.Map<ProductModel>(product);
        }
    }
}
