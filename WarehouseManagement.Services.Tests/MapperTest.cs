using AutoMapper;
using WarehouseManagement.Services.Mappers;
using Xunit;

namespace WarehouseManagement.Services.Tests
{
    public class MapperTest
    {
        [Fact]
        public void TestMapper()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MapperService>());
            configuration.AssertConfigurationIsValid();
        }
    }
}
