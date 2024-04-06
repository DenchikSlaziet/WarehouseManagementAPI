using FluentValidation;
using WarehouseManagement.General;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;
using WarehouseManagement.Services.Contracts.Contracts;
using WarehouseManagement.Services.Contracts.Exceptions;
using WarehouseManagement.Services.Contracts.Models;
using WarehouseManagement.Services.Contracts.ModelsRequest;
using WarehouseManagement.Services.Validators;

namespace WarehouseManagement.Services.Services
{
    public sealed class ValidatorService : IServiceValidator
    {
        private readonly Dictionary<Type, IValidator> _validators = new Dictionary<Type, IValidator>();

        public ValidatorService(IProductReadRepository productReadRepository, 
            IWarehouseUnitReadRepository warehouseUnitReadRepository)
        {
           _validators.Add(typeof(ProductModel), new ProductModelValidator());
           _validators.Add(typeof(WarehouseModelRequest), new WarehouseRequestModelValidator(warehouseUnitReadRepository));
           _validators.Add(typeof(WarehouseUnitModelRequest), new WarehouseUnitRequestModelValidator(productReadRepository));           
        }

        public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken)
            where TModel : class
        {
            var typeModel = typeof(TModel);

            if(!_validators.TryGetValue(typeModel, out var validator))
            {
                throw new InvalidOperationException($"Не найден валидатор для модели {typeModel}");
            }
            
            var context = new ValidationContext<TModel>(model);
            var result = await validator.ValidateAsync(context, cancellationToken);

            if(!result.IsValid)
            {
                throw new WarehouseManagmentValidationException(result.Errors.
                    Select(x => InvalidateItemModel.New(x.PropertyName, x.ErrorMessage)));
            }        
        }
    }
}
