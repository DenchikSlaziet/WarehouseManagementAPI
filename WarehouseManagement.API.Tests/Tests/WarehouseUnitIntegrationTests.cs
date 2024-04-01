using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using WarehouseManagement.API.Models.CreateRequest;
using WarehouseManagement.API.Models.Response;
using WarehouseManagement.API.Tests.Infrastructures;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Test.Extensions;
using Xunit;

namespace WarehouseManagement.API.Tests.Tests
{
    public class WarehouseUnitIntegrationTests : BaseIntegrationTest
    {
        public WarehouseUnitIntegrationTests(WarehouseManagementApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouseUnitCreateRequest = mapper.Map<WarehouseUnitCreateRequest>(TestDataGenerator.WarehouseUnitModelRequest());
            var product = TestDataGenerator.Product();
            warehouseUnitCreateRequest.ProductId = product.Id;

            await context.Products.AddAsync(product);
            await unitOfWork.SaveChangesAsync();


            // Act
            string data = JsonConvert.SerializeObject(warehouseUnitCreateRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/WarehouseUnit", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<WarehouseUnitResponse>(resultString);

            var warehouseUnitFirst = await context.WarehouseUnits.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            warehouseUnitFirst.Should()
                .BeEquivalentTo(warehouseUnitCreateRequest);
        }

        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouseUnit = TestDataGenerator.WarehouseUnit();
            var warehouseUnitModelRequest = TestDataGenerator.WarehouseUnitModelRequest(x => x.Id = warehouseUnit.Id);
            var product = TestDataGenerator.Product();
            warehouseUnit.ProductId = product.Id;
            warehouseUnitModelRequest.ProductId = product.Id;

            await context.WarehouseUnits.AddAsync(warehouseUnit);
            await context.Products.AddAsync(product);
            await unitOfWork.SaveChangesAsync();
           
            // Act
            string data = JsonConvert.SerializeObject(warehouseUnitModelRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/WarehouseUnit", contextdata);

            var warehouseUnitFirst = await context.WarehouseUnits.FirstAsync(x => x.Id == warehouseUnitModelRequest.Id);

            // Assert           
            warehouseUnitFirst.Should()
                .BeEquivalentTo(warehouseUnitModelRequest);
        }

        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouseUnit = TestDataGenerator.WarehouseUnit();
            var product = TestDataGenerator.Product();
            warehouseUnit.ProductId = product.Id;

            await context.Products.AddAsync(product);
            await context.WarehouseUnits.AddAsync(warehouseUnit);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/WarehouseUnit/{warehouseUnit.Id}");

            var warehouseUnitFirst = await context.WarehouseUnits.FirstAsync(x => x.Id == warehouseUnit.Id);

            // Assert
            warehouseUnitFirst.DeletedAt.Should()
                .NotBeNull();

            warehouseUnitFirst.Should()
            .BeEquivalentTo(new
            {
                warehouseUnit.Unit,
                warehouseUnit.Price,
                warehouseUnit.ProductId,
                warehouseUnit.Count
            });
        }

        [Fact]
        public async Task GetShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouseUnit1 = TestDataGenerator.WarehouseUnit();
            var warehouseUnit2 = TestDataGenerator.WarehouseUnit();
            var product = TestDataGenerator.Product();
            warehouseUnit1.ProductId = product.Id;
            warehouseUnit2.ProductId = product.Id;

            await context.Products.AddAsync(product);
            await context.WarehouseUnits.AddRangeAsync(warehouseUnit1, warehouseUnit2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/WarehouseUnit/{warehouseUnit1.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<WarehouseUnitResponse>(resultString);

            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    warehouseUnit1.Unit,
                    warehouseUnit1.Price,
                    warehouseUnit1.Count
                });
        }

        [Fact]
        public async Task GetAllShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var warehouseUnit1 = TestDataGenerator.WarehouseUnit();
            var warehouseUnit2 = TestDataGenerator.WarehouseUnit(x => x.DeletedAt = DateTimeOffset.Now);
            var product = TestDataGenerator.Product();
            warehouseUnit1.ProductId = product.Id;
            warehouseUnit2.ProductId = product.Id;

            await context.Products.AddAsync(product);
            await context.WarehouseUnits.AddRangeAsync(warehouseUnit1, warehouseUnit2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/WarehouseUnit");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<WarehouseUnitResponse>>(resultString);

            result.Should()
                .NotBeNull()
                .And
                .Contain(x => x.Id == warehouseUnit1.Id);

            result.Should()
                .NotBeNull()
                .And
                .NotContain(x => x.Id == warehouseUnit2.Id);
        }
    }
}
