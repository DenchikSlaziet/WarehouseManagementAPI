using AutoMapper;
using FluentAssertions;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Context.Tests;
using WarehouseManagement.Repositories.ReadRepositories;
using WarehouseManagement.Repositories.WriteRepositories;
using WarehouseManagement.Services.Contracts.Contracts;
using WarehouseManagement.Services.Contracts.Exceptions;
using WarehouseManagement.Services.Mappers;
using WarehouseManagement.Services.Services;
using WarehouseManagement.Test.Extensions;
using Xunit;

namespace WarehouseManagement.Services.Tests.ServiceTests
{
    public class ServiceWarehouseUnitTests : WarehouseManagementContextInMemory
    {
        private readonly IWarehouseUnitService warehouseUnitService;

        public ServiceWarehouseUnitTests() 
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperService());
            });

            var productReadRepository = new ProductReadRepository(Reader);
            var warehouseUnitReadRepository = new WarehouseUnitReadRepository(Reader);

            warehouseUnitService = new WarehouseUnitService(warehouseUnitReadRepository,
                new WarehouseUnitWriteRepository(WriterContext), productReadRepository,
                config.CreateMapper(), UnitOfWork, new ValidatorService(productReadRepository,
                warehouseUnitReadRepository));
        }

        /// <summary>
        /// Получение <see cref="WarehouseUnitModel"/> по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => warehouseUnitService.GetByIdAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<WarehouseUnit>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Получение <see cref="WarehouseUnitModel"/> по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.WarehouseUnit();
            var product = TestDataGenerator.Product();
            target.ProductId = product.Id;

            await Context.Products.AddAsync(product);
            await Context.WarehouseUnits.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitService.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(new
                {
                    target.Id,
                    target.Price,
                    target.Count,
                    target.Unit
                });
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{WarehouseUnitModel}"/> по идентификаторам возвращает пустую коллекцию
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await warehouseUnitService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение <see cref="WarehouseUnitModel"/> по идентификаторам возвращает данные
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnValues()
        {
            //Arrange
            var target = TestDataGenerator.WarehouseUnit();
            var product = TestDataGenerator.Product();
            target.ProductId = product.Id;

            await Context.Products.AddAsync(product);
            await Context.WarehouseUnits.AddRangeAsync(target,
                TestDataGenerator.WarehouseUnit(x => x.DeletedAt = DateTimeOffset.UtcNow));
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseUnitService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == target.Id);
        }

        /// <summary>
        /// Удаление несуществуюущего <see cref="WarehouseUnitModel"/>
        /// </summary>
        [Fact]
        public async Task DeletingNonExistentCinemaReturnExсeption()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => warehouseUnitService.DeleteAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<WarehouseUnit>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Удаление удаленного <see cref="WarehouseUnitModel"/>
        /// </summary>
        [Fact]
        public async Task DeletingDeletedCinemaReturnExсeption()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseUnit(x => x.DeletedAt = DateTime.UtcNow);
            await Context.WarehouseUnits.AddAsync(model);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            Func<Task> result = () => warehouseUnitService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<WarehouseUnit>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Удаление <see cref="WarehouseUnitModel"/>
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseUnit();
            await Context.WarehouseUnits.AddAsync(model);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => warehouseUnitService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.WarehouseUnits.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Добавление <see cref="WarehouseUnitModel"/>
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseUnitModelRequest();
            var product = TestDataGenerator.Product();
            model.ProductId = product.Id;

            await Context.Products.AddAsync(product);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => warehouseUnitService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.WarehouseUnits.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().BeNull();
        }

        /// <summary>
        /// Добавление не валидируемого <see cref="WarehouseUnitModel"/>
        /// </summary>
        [Fact]
        public async Task AddShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseUnitModelRequest();

            //Act
            Func<Task> act = () => warehouseUnitService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<WarehouseManagmentValidationException>();
        }

        /// <summary>
        /// Изменение несуществующего <see cref="WarehouseUnitModel"/>
        /// </summary>
        [Fact]
        public async Task EditShouldNotFoundException()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseUnitModelRequest();
            var product = TestDataGenerator.Product();
            model.ProductId = product.Id;

            await Context.Products.AddAsync(product);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => warehouseUnitService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<WarehouseUnit>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Изменение невалидируемого <see cref="WarehouseUnitModel"/>
        /// </summary>
        [Fact]
        public async Task EditShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseUnitModelRequest();
            var warehouseUnit = TestDataGenerator.WarehouseUnit(x => x.Id = model.Id);
            await Context.WarehouseUnits.AddAsync(warehouseUnit);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => warehouseUnitService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<WarehouseManagmentValidationException>();
        }

        /// <summary>
        /// Изменение <see cref="WarehouseUnitModel"/>
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseUnitModelRequest();
            var product = TestDataGenerator.Product();
            model.ProductId = product.Id;
            var warehouseUnit = TestDataGenerator.WarehouseUnit(x => x.Id = model.Id);

            await Context.Products.AddAsync(product);
            await Context.WarehouseUnits.AddAsync(warehouseUnit);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => warehouseUnitService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.WarehouseUnits.Single(x => x.Id == warehouseUnit.Id);
            entity.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    model.Id,
                    model.Price,
                    model.Count,
                    model.Unit
                });
        }
    }
}
