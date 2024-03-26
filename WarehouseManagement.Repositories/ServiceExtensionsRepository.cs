using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.General;
using WarehouseManagement.Repositories.Contracts.Anchors;

namespace WarehouseManagement.Repositories
{
    /// <summary>
    /// Расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceExtensionsRepository
    {
        /// <summary>
        /// Регистрация репозиториев
        /// </summary>
        public static void RegistrationRepository(this IServiceCollection service)
        {
            service.RegistrationOnInterface<IRepositoryAnchor>(ServiceLifetime.Scoped);
        }
    }
}
