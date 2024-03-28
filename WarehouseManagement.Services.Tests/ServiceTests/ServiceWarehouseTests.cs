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
    public class ServiceWarehouseTests : WarehouseManagementContextInMemory
    {
        private IWarehouseService warehouseService;

        public ServiceWarehouseTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperService());
            });

            warehouseService = new WarehouseService(new WarehouseReadRepository(Reader),
                new WarehouseWriteRepository(WriterContext), config.CreateMapper(),
                new ValidatorService(new ProductReadRepository(Reader),
                new WarehouseUnitReadRepository(Reader)), UnitOfWork,
                new WarehouseWarehouseUnitWriteRepository(WriterContext),
                new ProductReadRepository(Reader));            
        }

        /// <summary>
        /// Получение <see cref="WarehouseModel"/> по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => warehouseService.GetByIdAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<Warehouse>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Получение <see cref="WarehouseModel"/> по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.Warehouse();
            await Context.Warehouses.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseService.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(new
                {
                    target.Id,
                    target.Title,
                    target.Address                 
                });
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{WarehouseModel}"/> по идентификаторам возвращает пустую коллекцию
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await warehouseService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение <see cref="WarehouseModel"/> по идентификаторам возвращает данные
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnValues()
        {
            //Arrange
            var target = TestDataGenerator.Warehouse();

            await Context.Warehouses.AddRangeAsync(target,
                TestDataGenerator.Warehouse(x => x.DeletedAt = DateTimeOffset.UtcNow));
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await warehouseService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == target.Id);
        }

        /// <summary>
        /// Удаление несуществуюущего <see cref="WarehouseModel"/>
        /// </summary>
        [Fact]
        public async Task DeletingNonExistentCinemaReturnExсeption()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => warehouseService.DeleteAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<Warehouse>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Удаление удаленного <see cref="WarehouseModel"/>
        /// </summary>
        [Fact]
        public async Task DeletingDeletedCinemaReturnExсeption()
        {
            //Arrange
            var model = TestDataGenerator.Warehouse(x => x.DeletedAt = DateTime.UtcNow);
            await Context.Warehouses.AddAsync(model);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            Func<Task> result = () => warehouseService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<Warehouse>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Удаление <see cref="WarehouseModel"/>
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.Warehouse();
            await Context.Warehouses.AddAsync(model);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => warehouseService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Warehouses.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Добавление <see cref="WarehouseModel"/>
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseModelRequest(x => 
                x.WarehouseUnitModelIds = new List<Guid> { });

            //Act
            Func<Task> act = () => warehouseService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Warehouses.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().BeNull();
        }

        /// <summary>
        /// Добавление не валидируемого <see cref="WarehouseModel"/>
        /// </summary>
        [Fact]
        public async Task AddShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseModelRequest(x => x.Title = string.Empty);

            //Act
            Func<Task> act = () => warehouseService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<WarehouseManagmentValidationException>();
        }

        /// <summary>
        /// Изменение несуществующего <see cref="WarehouseModel"/>
        /// </summary>
        [Fact]
        public async Task EditShouldNotFoundException()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseModelRequest();

            //Act
            Func<Task> act = () => warehouseService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<Warehouse>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Изменение невалидируемого <see cref="WarehouseModel"/>
        /// </summary>
        [Fact]
        public async Task EditShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseModelRequest(x => x.Title = string.Empty);
            var warehouse = TestDataGenerator.Warehouse(x => x.Id = model.Id);
            await Context.Warehouses.AddAsync(warehouse);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => warehouseService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<WarehouseManagmentValidationException>();
        }

        /// <summary>
        /// Изменение <see cref="WarehouseModel"/>
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.WarehouseModelRequest(x => x.WarehouseUnitModelIds = new List<Guid> { });
            var warehouse = TestDataGenerator.Warehouse(x => x.Id = model.Id);
            await Context.Warehouses.AddAsync(warehouse);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => warehouseService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Warehouses.Single(x => x.Id == warehouse.Id);
            entity.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    model.Id,
                    model.Title,
                    model.Address,
                });
        }
    }
}

