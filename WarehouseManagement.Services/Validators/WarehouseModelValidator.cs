using FluentValidation;
using WarehouseManagement.Services.Contracts.Models;

namespace WarehouseManagement.Services.Validators
{
    /// <summary>
    /// Валидатор <<see cref="WarehouseModel"/>
    /// </summary>
    internal class WarehouseModelValidator : AbstractValidator<WarehouseModel>
    {
        public WarehouseModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
               .NotNull().WithMessage(MessageForValidation.DefaultMessage)
               .Length(2, 50).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Address).NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
               .NotNull().WithMessage(MessageForValidation.DefaultMessage)
               .Length(10, 100).WithMessage(MessageForValidation.LengthMessage);
        }
    }
}
