using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using WarehouseManagement.API.Models.CreateRequest;
using WarehouseManagement.API.Models.Request;
using WarehouseManagement.API.Models.Response;
using WarehouseManagement.API.Tests.Infrastructures;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Test.Extensions;
using Xunit;

namespace WarehouseManagement.API.Tests.Tests
{
    public class WarehoouseIntegrationTests : BaseIntegrationTest
    {
        public WarehoouseIntegrationTests(WarehouseManagementApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouseCreateRequest = mapper.Map<WarehouseCreateRequest>(TestDataGenerator.WarehouseModelRequest());
            var warehouseUnit = TestDataGenerator.WarehouseUnit();
            var product = TestDataGenerator.Product();
            warehouseUnit.ProductId = product.Id;
            warehouseCreateRequest.WarehouseUnitModelIds = new[] { warehouseUnit.Id };

            await context.Products.AddAsync(product);
            await context.WarehouseUnits.AddAsync(warehouseUnit);
            await unitOfWork.SaveChangesAsync();

            // Act
            string data = JsonConvert.SerializeObject(warehouseCreateRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Warehouse", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<WarehouseResponse>(resultString);

            var warehouseFirst = await context.Warehouses.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            warehouseFirst.Should()
                .BeEquivalentTo(new
                {
                    warehouseCreateRequest.Title,
                    warehouseCreateRequest.Address
                });
        }

        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouse = TestDataGenerator.Warehouse();
            var warehouseModelRequest = TestDataGenerator.WarehouseModelRequest(x => x.Id = warehouse.Id);
            var warehouseUnit = TestDataGenerator.WarehouseUnit();
            var product = TestDataGenerator.Product();
            warehouseUnit.ProductId = product.Id;
            warehouseModelRequest.WarehouseUnitModelIds = new[] { warehouseUnit.Id };

            await context.Products.AddAsync(product);
            await context.Warehouses.AddAsync(warehouse);
            await context.WarehouseUnits.AddAsync(warehouseUnit);
            await unitOfWork.SaveChangesAsync();

            // Act
            string data = JsonConvert.SerializeObject(warehouseModelRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/Warehouse", contextdata);

            var warehouseFirst = await context.Warehouses.FirstAsync(x => x.Id == warehouseModelRequest.Id);

            // Assert           
            warehouseFirst.Should()
                .BeEquivalentTo(new
                {
                    warehouseModelRequest.Title,
                    warehouseModelRequest.Address
                });
        }

        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouse = TestDataGenerator.Warehouse();
            await context.Warehouses.AddAsync(warehouse);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/Warehouse/{warehouse.Id}");

            var warehouseFirst = await context.Warehouses.FirstAsync(x => x.Id == warehouse.Id);

            // Assert
            warehouseFirst.DeletedAt.Should()
                .NotBeNull();

            warehouseFirst.Should()
            .BeEquivalentTo(new
            {
                warehouse.Title,
                warehouse.Address
            });
        }

        [Fact]
        public async Task GetShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouse1 = TestDataGenerator.Warehouse();
            var warehouse2 = TestDataGenerator.Warehouse();

            await context.Warehouses.AddRangeAsync(warehouse1, warehouse2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/Warehouse/{warehouse1.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<WarehouseResponse>(resultString);

            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    warehouse1.Id,
                    warehouse1.Title,
                    warehouse1.Address
                });
        }

        [Fact]
        public async Task GetAllShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouse1 = TestDataGenerator.Warehouse();
            var warehouse2 = TestDataGenerator.Warehouse(x => x.DeletedAt = DateTimeOffset.Now);

            await context.Warehouses.AddRangeAsync(warehouse1, warehouse2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/Warehouse");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<WarehouseResponse>>(resultString);

            result.Should()
                .NotBeNull()
                .And
                .Contain(x => x.Id == warehouse1.Id);

            result.Should()
                .NotBeNull()
                .And
                .NotContain(x => x.Id == warehouse2.Id);
        }
    }
}
