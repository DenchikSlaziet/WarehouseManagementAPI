using FluentValidation.TestHelper;
using WarehouseManagement.Context.Tests;
using WarehouseManagement.Repositories.ReadRepositories;
using WarehouseManagement.Services.Validators;
using WarehouseManagement.Test.Extensions;
using Xunit;

namespace WarehouseManagement.Services.Tests.ValidatorsTests
{
    public class WarehouseRequestModelValidatorTest : WarehouseManagementContextInMemory
    {
        private readonly WarehouseRequestModelValidator validator;

        public WarehouseRequestModelValidatorTest()
        {
            validator = new WarehouseRequestModelValidator(new WarehouseUnitReadRepository(Reader));            
        }

        /// <summary>
        /// Тест на наличие ошибок
        /// </summary>
        [Fact]
        public async Task ValidatorShouldErrorAsync()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseModelRequest(x =>
            {
                x.Title = string.Empty;
                x.Address = string.Empty;
                x.WarehouseUnitModelIds = new List<Guid>
                {
                    Guid.NewGuid()
                };
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
            var model = TestDataGenerator.WarehouseModelRequest();
            var product = TestDataGenerator.Product();
            var warehouseUnit = TestDataGenerator.WarehouseUnit(x => x.ProductId = product.Id);
            model.WarehouseUnitModelIds = new List<Guid> { warehouseUnit.Id };

            await Context.Products.AddAsync(product);
            await Context.WarehouseUnits.AddAsync(warehouseUnit);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
