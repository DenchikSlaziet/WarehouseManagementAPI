using FluentValidation;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;
using WarehouseManagement.Services.Contracts.ModelsRequest;

namespace WarehouseManagement.Services.Validators
{
    /// <summary>
    /// Валидатор <<see cref="WarehouseModelRequest"/>
    /// </summary>
    public class WarehouseRequestModelValidator : AbstractValidator<WarehouseModelRequest>
    {
        private readonly IWarehouseUnitReadRepository warehouseUnitReadRepository;

        public WarehouseRequestModelValidator(IWarehouseUnitReadRepository warehouseUnitReadRepository)
        {
            this.warehouseUnitReadRepository = warehouseUnitReadRepository;

            RuleFor(x => x.Title).NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
               .NotNull().WithMessage(MessageForValidation.DefaultMessage)
               .Length(2, 50).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Address).NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
               .NotNull().WithMessage(MessageForValidation.DefaultMessage)
               .Length(10, 100).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.WarehouseUnitModelIds)
                .NotNull().WithMessage(MessageForValidation.DefaultMessage);

            RuleForEach(x => x.WarehouseUnitModelIds)                
                .MustAsync(async (x, cancellationToken) => await this.warehouseUnitReadRepository.IsNotNullAsync(x, cancellationToken))
                .WithMessage(MessageForValidation.NotFoundGuidMessage);
        }
    }
}
