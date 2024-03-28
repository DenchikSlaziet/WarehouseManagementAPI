using FluentValidation.TestHelper;
using WarehouseManagement.Services.Validators;
using WarehouseManagement.Test.Extensions;
using Xunit;

namespace WarehouseManagement.Services.Tests.ValidatorsTests
{
    public class ProductValidatorTests
    {
        private readonly ProductModelValidator productModelValidator;

        public ProductValidatorTests()
        {
            productModelValidator = new ProductModelValidator();            
        }

        /// <summary>
        /// Тест на наличие ошибок
        /// </summary>
        [Fact]
        public void ValidatorShouldError()
        {
            //Arrange
            var model = TestDataGenerator.ProductModel(x =>
            {
                x.Title = string.Empty;
                x.Description = "1";
                
            });

            // Act
            var result = productModelValidator.TestValidate(model);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        /// <summary>
        /// Тест на отсутствие ошибок
        /// </summary>
        [Fact]
        public void ValidatorShouldSuccess()
        {
            //Arrange
            var model = TestDataGenerator.ProductModel();

            // Act
            var result = productModelValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
