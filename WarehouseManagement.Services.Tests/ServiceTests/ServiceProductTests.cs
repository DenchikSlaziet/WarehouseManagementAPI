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
    public class ServiceProductTests : WarehouseManagementContextInMemory
    {
        private readonly IProductService productService;

        public ServiceProductTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperService());
            });

            var productReadRepository = new ProductReadRepository(Reader);

            productService = new ProductService(productReadRepository,
                new ProductWriteRepository(WriterContext), config.CreateMapper(), UnitOfWork,
                new ValidatorService(productReadRepository, new WarehouseUnitReadRepository(Reader)));
        }

        /// <summary>
        /// Получение <see cref="ProductModel"/> по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => productService.GetByIdAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<Product>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Получение <see cref="ProductModel"/> по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.Product();
            await Context.Products.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await productService.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(new
                {
                    target.Id,
                    target.Title,
                    target.Description
                });
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{ProductModel}"/> по идентификаторам возвращает пустую коллекцию
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await productService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение <see cref="ProductModel"/> по идентификаторам возвращает данные
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
            var result = await productService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == target.Id);
        }

        /// <summary>
        /// Удаление несуществуюущего <see cref="ProductModel"/>
        /// </summary>
        [Fact]
        public async Task DeletingNonExistentCinemaReturnExсeption()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => productService.DeleteAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<Product>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Удаление удаленного <see cref="ProductModel"/>
        /// </summary>
        [Fact]
        public async Task DeletingDeletedCinemaReturnExсeption()
        {
            //Arrange
            var model = TestDataGenerator.Product(x => x.DeletedAt = DateTime.UtcNow);
            await Context.Products.AddAsync(model);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            Func<Task> result = () => productService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<Product>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Удаление <see cref="ProductModel"/>
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.Product();
            await Context.Products.AddAsync(model);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => productService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Products.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Добавление <see cref="ProductModel"/>
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.ProductModel();

            //Act
            Func<Task> act = () => productService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Products.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().BeNull();
        }

        /// <summary>
        /// Добавление не валидируемого <see cref="ProductModel"/>
        /// </summary>
        [Fact]
        public async Task AddShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.ProductModel(x => x.Description = "1");

            //Act
            Func<Task> act = () => productService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<WarehouseManagmentValidationException>();
        }

        /// <summary>
        /// Изменение несуществующего <see cref="ProductModel"/>
        /// </summary>
        [Fact]
        public async Task EditShouldNotFoundException()
        {
            //Arrange
            var model = TestDataGenerator.ProductModel();

            //Act
            Func<Task> act = () => productService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<WarehouseManagmentEntityNotFoundException<Product>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Изменение невалидируемого <see cref="ProductModel"/>
        /// </summary>
        [Fact]
        public async Task EditShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.ProductModel(x => x.Description = "1");

            //Act
            Func<Task> act = () => productService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<WarehouseManagmentValidationException>();
        }

        /// <summary>
        /// Изменение <see cref="ProductModel"/>
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.ProductModel();
            var film = TestDataGenerator.Product(x => x.Id = model.Id);
            await Context.Products.AddAsync(film);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => productService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Products.Single(x => x.Id == film.Id);
            entity.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    model.Id,
                    model.Title,
                    model.Description                   
                });
        }
    }
}
