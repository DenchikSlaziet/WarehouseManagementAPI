using Xunit;

namespace WarehouseManagement.API.Tests.Infrastructures
{
    [CollectionDefinition(nameof(WarehouseManagementApiTestCollection))]
    public class WarehouseManagementApiTestCollection : ICollectionFixture<WarehouseManagementApiFixture>
    {
    }
}
