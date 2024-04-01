using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context;
using WarehouseManagement.Context.Contracts;
using Xunit;

namespace WarehouseManagement.API.Tests.Infrastructures
{
    public class WarehouseManagementApiFixture : IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory factory;
        private WarehouseManagementContext? warehouseManagementContext;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="WarehouseManagementApiFixture"/>
        /// </summary>
        public WarehouseManagementApiFixture()
        {
            factory = new CustomWebApplicationFactory();
        }

        Task IAsyncLifetime.InitializeAsync() => WarehouseManagementContext.Database.MigrateAsync();

        async Task IAsyncLifetime.DisposeAsync()
        {
            await WarehouseManagementContext.Database.EnsureDeletedAsync();
            await WarehouseManagementContext.Database.CloseConnectionAsync();
            await WarehouseManagementContext.DisposeAsync();
            await factory.DisposeAsync();
        }

        public CustomWebApplicationFactory Factory => factory;

        public IWarehouseManagementContext Context => WarehouseManagementContext;

        public IUnitOfWork UnitOfWork => WarehouseManagementContext;

        internal WarehouseManagementContext WarehouseManagementContext
        {
            get
            {
                if (warehouseManagementContext != null)
                {
                    return warehouseManagementContext;
                }

                var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
                warehouseManagementContext = scope.ServiceProvider.GetRequiredService<WarehouseManagementContext>();
                return warehouseManagementContext;
            }
        }
    }
}
