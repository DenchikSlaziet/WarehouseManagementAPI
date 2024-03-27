using FluentValidation;
using WarehouseManagement.Services.Contracts.Models;

namespace WarehouseManagement.Services.Validators
{
    /// <summary>
    /// Валидатор <<see cref="ProductModel"/>
    /// </summary>
    internal class ProductModelValidator : AbstractValidator<ProductModel>
    {
        public ProductModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
               .NotNull().WithMessage(MessageForValidation.DefaultMessage)
               .Length(2, 40).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Description).Length(3, 300).WithMessage(MessageForValidation.LengthMessage)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
