using FluentAssertions;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Context.Tests;
using WarehouseManagement.Repositories.Contracts.ReadRepositories;
using WarehouseManagement.Repositories.ReadRepositories;
using WarehouseManagement.Test.Extensions;
using Xunit;

namespace WarehouseManagement.Repositories.Tests.Tests
{
    public class WarehouseUnitReadTests : WarehouseManagementContextInMemory
    {
        private readonly IWarehouseUnitReadRepository warehouseUnitReadRepository;

        public WarehouseUnitReadTests()
        {
            warehouseUnitReadRepository = new WarehouseUnitReadRepository(Context);
        }

        /// <summary>
        /// Возвращает пустой список SKU
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await warehouseUnitReadRepository.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Возвращает список SKU
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnValues()
        {
            //Arrange
            var target = TestDataGenerator.WarehouseUnit();

            await Context.WarehouseUnits.AddRangeAsync(target,
                TestDataGenerator.WarehouseUnit(x => x.DeletedAt = DateTimeOffset.UtcNow));
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == target.Id);
        }

        /// <summary>
        /// Получение SKU по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var target = TestDataGenerator.WarehouseUnit(x => x.DeletedAt = DateTimeOffset.Now);
            await Context.WarehouseUnits.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Получение SKU по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.WarehouseUnit();
            await Context.WarehouseUnits.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(target);
        }

        /// <summary>
        /// Получение списка SKU по идентификаторам возвращает пустую коллекцию
        /// </summary>
        [Fact]
        public async Task GetByIdsShouldReturnEmpty()
        {
            //Arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();

            // Act
            var result = await warehouseUnitReadRepository.GetByIdsAsync(new[] { id1, id2, id3 }, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение списка SKU по идентификаторам возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdsShouldReturnValue()
        {
            //Arrange
            var target1 = TestDataGenerator.WarehouseUnit();
            var target2 = TestDataGenerator.WarehouseUnit(x => x.DeletedAt = DateTimeOffset.UtcNow);
            var target3 = TestDataGenerator.WarehouseUnit();
            var target4 = TestDataGenerator.WarehouseUnit();
            await Context.WarehouseUnits.AddRangeAsync(target1, target2, target3, target4);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.GetByIdsAsync(new[] { target1.Id, target2.Id, target4.Id }, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(2)
                .And.ContainKey(target1.Id)
                .And.ContainKey(target4.Id);
        }

        /// <summary>
        /// Поиск SKU в коллекции по идентификатору (true)
        /// </summary>
        [Fact]
        public async Task IsNotNullEntityReturnTrue()
        {
            //Arrange
            var target1 = TestDataGenerator.WarehouseUnit();
            await Context.WarehouseUnits.AddAsync(target1);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.IsNotNullAsync(target1.Id, CancellationToken);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Поиск SKU в коллекции по идентификатору (false)
        /// </summary>
        [Fact]
        public async Task IsNotNullEntityReturnFalse()
        {
            //Arrange
            var target1 = Guid.NewGuid();

            // Act
            var result = await warehouseUnitReadRepository.IsNotNullAsync(target1, CancellationToken);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Поиск удаленного SKU в коллекции по идентификатору
        /// </summary>
        [Fact]
        public async Task IsNotNullDeletedEntityReturnFalse()
        {
            //Arrange
            var target1 = TestDataGenerator.WarehouseUnit(x => x.DeletedAt = DateTimeOffset.UtcNow);
            await Context.WarehouseUnits.AddAsync(target1);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.IsNotNullAsync(target1.Id, CancellationToken);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Поиск склада по идентификатору SKU (empty)
        /// </summary>
        [Fact]
        public async Task GetWarehouseByWarehouseUnitIdReturnEmpty()
        {
            //Arrange
            var target1 = TestDataGenerator.WarehouseUnit();
            await Context.WarehouseUnits.AddAsync(target1);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.GetWarehouseByWarehouseUnitId(target1.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Поиск склада по идентификатору SKU
        /// </summary>
        [Fact]
        public async Task GetWarehouseByWarehouseUnitIdReturnValues()
        {
            //Arrange
            var warehouseUnit1 = TestDataGenerator.WarehouseUnit();
            var warehouseUnit2 = TestDataGenerator.WarehouseUnit();
            var warehouse1 = TestDataGenerator.Warehouse();
            var warehouse2 = TestDataGenerator.Warehouse();
            var warehouseWarehouseUnit1 = new WarehouseWarehouseUnit
            {
                WarehouseId = warehouse1.Id,
                WarehouseUnitId = warehouseUnit1.Id,
            };
            var warehouseWarehouseUnit2 = new WarehouseWarehouseUnit
            {
                WarehouseId = warehouse2.Id,
                WarehouseUnitId = warehouseUnit2.Id,
            };

            await Context.WarehouseUnits.AddRangeAsync(warehouseUnit1, warehouseUnit2);
            await Context.Warehouses.AddRangeAsync(warehouse1, warehouse2);
            await Context.WarehouseWarehouseUnits.AddRangeAsync(warehouseWarehouseUnit1,
                warehouseWarehouseUnit2);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.GetWarehouseByWarehouseUnitId(warehouseUnit1.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == warehouse1.Id);
        }

        /// <summary>
        /// Поиск сущностей из промежуточной таблицы по идентификатору SKU (empty)
        /// </summary>
        [Fact]
        public async Task GetDependenceEntityByWarehouseIdReturnEmpty()
        {
            //Arrange
            var target1 = TestDataGenerator.WarehouseUnit();
            await Context.WarehouseUnits.AddAsync(target1);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.GetDependenceEntityByWarehouseUnitId(target1.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Поиск сущностей из промежуточной таблицы по идентификатору SKU
        /// </summary>
        [Fact]
        public async Task GetDependenceEntityByWarehouseIdReturnValues()
        {
            //Arrange
            var warehouseUnit1 = TestDataGenerator.WarehouseUnit();
            var warehouseUnit2 = TestDataGenerator.WarehouseUnit();
            var warehouse1 = TestDataGenerator.Warehouse();
            var warehouse2 = TestDataGenerator.Warehouse();
            var warehouseWarehouseUnit1 = new WarehouseWarehouseUnit
            {
                WarehouseId = warehouse1.Id,
                WarehouseUnitId = warehouseUnit1.Id,
            };
            var warehouseWarehouseUnit2 = new WarehouseWarehouseUnit
            {
                WarehouseId = warehouse2.Id,
                WarehouseUnitId = warehouseUnit2.Id,
            };

            await Context.WarehouseUnits.AddRangeAsync(warehouseUnit1, warehouseUnit2);
            await Context.Warehouses.AddRangeAsync(warehouse1, warehouse2);
            await Context.WarehouseWarehouseUnits.AddRangeAsync(warehouseWarehouseUnit1,
                warehouseWarehouseUnit2);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitReadRepository.GetDependenceEntityByWarehouseUnitId(warehouseUnit1.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.ContainSingle(x => x.WarehouseUnitId == warehouseUnit1.Id &&
                x.WarehouseId == warehouse1.Id);
        }
    }
}
