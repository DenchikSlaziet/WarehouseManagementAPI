using FluentValidation;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;
using WarehouseManagement.Services.Contracts.ModelsRequest;

namespace WarehouseManagement.Services.Validators
{
    /// <summary>
    /// Валидатор <<see cref="WarehouseUnitModelRequest"/>
    /// </summary>
    public class WarehouseUnitRequestModelValidator : AbstractValidator<WarehouseUnitModelRequest>
    {
        private readonly IProductReadRepository _productReadRepository;

        public WarehouseUnitRequestModelValidator(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;

            RuleFor(x => x.Unit)
               .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
               .NotNull().WithMessage(MessageForValidation.DefaultMessage)
               .Length(2, 50).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Price)
                .InclusiveBetween(100, 50000).WithMessage(MessageForValidation.InclusiveBetweenMessage);

            RuleFor(x => x.Count)
               .GreaterThan(0).WithMessage(MessageForValidation.InclusiveBetweenMessage);

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .MustAsync(async (x, cancellationToken) => await _productReadRepository.IsNotNullAsync(x, cancellationToken))
                .WithMessage(MessageForValidation.NotFoundGuidMessage);
        }
    }
}
