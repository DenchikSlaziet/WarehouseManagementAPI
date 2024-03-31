using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.General;
using WarehouseManagement.Services.Anchors;
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
        public static void RegistrationServices(this IServiceCollection service)
        {
            service.AddTransient<IServiceValidator, ValidatorService>();
            service.RegistrationOnInterface<IServiceAnchor>(ServiceLifetime.Scoped);           
        }
    }
}
