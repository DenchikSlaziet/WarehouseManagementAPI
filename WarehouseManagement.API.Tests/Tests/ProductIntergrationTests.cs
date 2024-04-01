using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using WarehouseManagement.API.Models.CreateRequest;
using WarehouseManagement.API.Models.Request;
using WarehouseManagement.API.Models.Response;
using WarehouseManagement.API.Tests.Infrastructures;
using WarehouseManagement.Test.Extensions;
using Xunit;

namespace WarehouseManagement.API.Tests.Tests
{
    public class ProductIntergrationTests : BaseIntegrationTest
    {
        public ProductIntergrationTests(WarehouseManagementApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var product = mapper.Map<ProductCreateRequest>(TestDataGenerator.ProductModel());

            // Act
            string data = JsonConvert.SerializeObject(product);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Product", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductResponse>(resultString);

            var productFirst = await context.Products.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            productFirst.Should()
                .BeEquivalentTo(product);
        }

        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var product = TestDataGenerator.Product();
            await context.Products.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            var productRequest = mapper.Map<ProductRequest>(TestDataGenerator.ProductModel(x => x.Id = product.Id));

            // Act
            string data = JsonConvert.SerializeObject(productRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/Product", contextdata);

            var productFirst = await context.Products.FirstAsync(x => x.Id == productRequest.Id);

            // Assert           
            productFirst.Should()
                .BeEquivalentTo(productRequest);
        }

        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var product = TestDataGenerator.Product();
            await context.Products.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/Product/{product.Id}");

            var productFirst = await context.Products.FirstAsync(x => x.Id == product.Id);

            // Assert
            productFirst.DeletedAt.Should()
                .NotBeNull();

            productFirst.Should()
            .BeEquivalentTo(new
            {
                product.Id,
                product.Title,
                product.Description
            });
        }

        [Fact]
        public async Task GetShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var product1 = TestDataGenerator.Product();
            var product2 = TestDataGenerator.Product();

            await context.Products.AddRangeAsync(product1, product2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/Product/{product1.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductResponse>(resultString);

            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    product1.Id,
                    product1.Title,
                    product1.Description
                });
        }

        [Fact]
        public async Task GetAllShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var product1 = TestDataGenerator.Product();
            var product2 = TestDataGenerator.Product(x => x.DeletedAt = DateTimeOffset.Now);

            await context.Products.AddRangeAsync(product1, product2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/Product");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ProductResponse>>(resultString);

            result.Should()
                .NotBeNull()
                .And
                .Contain(x => x.Id == product1.Id);

            result.Should()
                .NotBeNull()
                .And
                .NotContain(x => x.Id == product2.Id);
        }
    }
}
