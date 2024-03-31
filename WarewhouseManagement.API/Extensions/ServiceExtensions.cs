using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context;
using WarehouseManagement.Repositories;
using WarehouseManagement.Services;
using WarehouseManagement.Services.Mappers;
using WarehouseManagement.API.Mappers;

namespace WarehouseManagement.API.Extensions
{
    /// <summary>
    /// Расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Регистрирует все сервисы, репозитории и все что нужно для контекста
        /// </summary>
        public static void RegistrationSRC(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(APIMappers), typeof(MapperService));
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IDbWriterContext, DbWriterContext>();
            services.RegistrationContext();
            services.RegistrationRepository();
            services.RegistrationServices();
        }

        /// <summary>
        /// Включает фильтры и ставит шрифт на перечесления
        /// </summary>
        /// <param name="services"></param>
        public static void RegistrationControllers(this IServiceCollection services)
        {
            services.AddControllers(x =>
            {
                x.Filters.Add<WarehoseManagementExceptionFilter>();
            })
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        CamelCaseText = false
                    });
                })
                .AddControllersAsServices();
        }

        /// <summary>
        /// Настройки свагера
        /// </summary>
        public static void RegistrationSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Product", new OpenApiInfo { Title = "Продукты", Version = "v1" });
                c.SwaggerDoc("Warehouse", new OpenApiInfo { Title = "Склады", Version = "v1" });
                c.SwaggerDoc("WarehouseUnit", new OpenApiInfo { Title = "SKU", Version = "v1" });

                var filePath = Path.Combine(AppContext.BaseDirectory, "WarehouseManagement.API.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        /// Настройки свагера
        /// </summary>
        public static void CustomizeSwaggerUI(this WebApplication web)
        {
            web.UseSwagger();
            web.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("Product/swagger.json", "Продукты");
                x.SwaggerEndpoint("Warehouse/swagger.json", "Склады");
                x.SwaggerEndpoint("WarehouseUnit/swagger.json", "SKU");
            });
        }
    }
}
