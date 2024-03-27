using FluentAssertions;
using WarehouseManagement.Context.Tests;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;
using WarehouseManagement.Repositories.ReadRepositories;
using WarehouseManagement.Test.Extensions;
using Xunit;

namespace WarehouseManagement.Repositories.Tests.Tests
{
    public class ProductReadTests : WarehouseManagementContextInMemory
    {
        private readonly IProductReadRepository productReadRepository;

        public ProductReadTests()
        {
            productReadRepository = new ProductReadRepository(Reader);
        }

        /// <summary>
        /// Возвращает пустой список продуктов
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await productReadRepository.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Возвращает список товаров
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnValues()
        {
            //Arrange
            var target = TestDataGenerator.Product();

            await Context.Products.AddRangeAsync(target,
                TestDataGenerator.Product(x => x.DeletedAt = DateTimeOffset.UtcNow));
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await productReadRepository.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == target.Id);
        }

        /// <summary>
        /// Получение товаров по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var target = TestDataGenerator.Product(x => x.DeletedAt = DateTimeOffset.Now);
            await Context.Products.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await productReadRepository.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Получение товаров по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.Product();
            await Context.Products.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await productReadRepository.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(target);
        }

        /// <summary>
        /// Получение списка товаров по идентификаторам возвращает пустую коллекцию
        /// </summary>
        [Fact]
        public async Task GetByIdsShouldReturnEmpty()
        {
            //Arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();

            // Act
            var result = await productReadRepository.GetByIdsAsync(new[] { id1, id2, id3 }, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение списка товаров по идентификаторам возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdsShouldReturnValue()
        {
            //Arrange
            var target1 = TestDataGenerator.Product();
            var target2 = TestDataGenerator.Product(x => x.DeletedAt = DateTimeOffset.UtcNow);
            var target3 = TestDataGenerator.Product();
            var target4 = TestDataGenerator.Product();
            await Context.Products.AddRangeAsync(target1, target2, target3, target4);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await productReadRepository.GetByIdsAsync(new[] { target1.Id, target2.Id, target4.Id }, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(2)
                .And.ContainKey(target1.Id)
                .And.ContainKey(target4.Id);
        }

        /// <summary>
        /// Поиск товара в коллекции по идентификатору (true)
        /// </summary>
        [Fact]
        public async Task IsNotNullEntityReturnTrue()
        {
            //Arrange
            var target1 = TestDataGenerator.Product();
            await Context.Products.AddAsync(target1);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await productReadRepository.IsNotNullAsync(target1.Id, CancellationToken);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Поиск товара в коллекции по идентификатору (false)
        /// </summary>
        [Fact]
        public async Task IsNotNullEntityReturnFalse()
        {
            //Arrange
            var target1 = Guid.NewGuid();

            // Act
            var result = await productReadRepository.IsNotNullAsync(target1, CancellationToken);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Поиск удаленного товара в коллекции по идентификатору
        /// </summary>
        [Fact]
        public async Task IsNotNullDeletedEntityReturnFalse()
        {
            //Arrange
            var target1 = TestDataGenerator.Product(x => x.DeletedAt = DateTimeOffset.UtcNow);
            await Context.Products.AddAsync(target1);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await productReadRepository.IsNotNullAsync(target1.Id, CancellationToken);

            // Assert
            result.Should().BeFalse();
        }

    }
}
