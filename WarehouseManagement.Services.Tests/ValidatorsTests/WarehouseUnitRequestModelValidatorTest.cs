using FluentValidation.TestHelper;
using WarehouseManagement.Context.Tests;
using WarehouseManagement.Repositories.ReadRepositories;
using WarehouseManagement.Services.Validators;
using WarehouseManagement.Test.Extensions;
using Xunit;

namespace WarehouseManagement.Services.Tests.ValidatorsTests
{
    public class WarehouseUnitRequestModelValidatorTest : WarehouseManagementContextInMemory
    {
        private readonly WarehouseUnitRequestModelValidator validator;

        public WarehouseUnitRequestModelValidatorTest()
        {
            validator = new WarehouseUnitRequestModelValidator(new ProductReadRepository(Reader));
        }

        /// <summary>
        /// Тест на наличие ошибок
        /// </summary>
        [Fact]
        public async Task ValidatorShouldErrorAsync()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseUnitModelRequest(x =>
            {
                x.Price = 1;
                x.Unit = string.Empty;
                x.Count = -1;
                x.ProductId = Guid.NewGuid();
            });

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        /// <summary>
        /// Тест на отсутствие ошибок
        /// </summary>
        [Fact]
        public async Task ValidatorShouldSuccessAsync()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseUnitModelRequest();
            var product = TestDataGenerator.Product();
            model.ProductId = product.Id;

            await Context.Products.AddAsync(product);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
