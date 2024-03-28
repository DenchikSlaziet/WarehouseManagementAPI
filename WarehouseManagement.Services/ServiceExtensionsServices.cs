using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WarehouseManagement.General;
using WarehouseManagement.Services.Contracts.Anchors;
using WarehouseManagement.Services.Contracts.Contracts;
using WarehouseManagement.Services.Services;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceExtensionsServices
    {
        /// <summary>
        /// Регистрация сервисов и валидатора
        /// </summary>
        public static void RegistrationRepository(this IServiceCollection service)
        {
            service.TryAddTransient<IServiceValidator>(provider => provider.GetRequiredService<ValidatorService>());
            service.RegistrationOnInterface<IServiceAnchor>(ServiceLifetime.Scoped);
        }
    }
}
