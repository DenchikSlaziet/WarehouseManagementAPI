using AutoMapper;
using WarehouseManagement.API.Mappers;
using Xunit;

namespace WarehouseManagement.API.Tests.Tests
{
    public class MapperTests
    {
        [Fact]
        public void TestMapper()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<APIMappers>());
            configuration.AssertConfigurationIsValid();
        }
    }
}
