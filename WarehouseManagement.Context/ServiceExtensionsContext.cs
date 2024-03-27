using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts;

namespace WarehouseManagement.Context
{
    /// <summary>
    /// Расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceExtensionsContext
    {
        /// <summary>
        /// Регистрация контекста
        /// </summary>
        public static void RegistrationContext(this IServiceCollection service)
        {
            service.TryAddScoped<IWarehouseManagementContext>(provider => provider.GetRequiredService<WarehouseManagementContext>());
            service.TryAddScoped<IDbRead>(provider => provider.GetRequiredService<WarehouseManagementContext>());
            service.TryAddScoped<IDbWriter>(provider => provider.GetRequiredService<WarehouseManagementContext>());
            service.TryAddScoped<IUnitOfWork>(provider => provider.GetRequiredService<WarehouseManagementContext>());
        }
    }
}
