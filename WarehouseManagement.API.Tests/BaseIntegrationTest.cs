using AutoMapper;
using WarehouseManagement.API.Mappers;
using WarehouseManagement.API.Tests.Infrastructures;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Contracts;
using WarehouseManagement.Services.Mappers;
using Xunit;

namespace WarehouseManagement.API.Tests
{
    /// <summary>
    /// Базовый класс для тестов
    /// </summary>
    [Collection(nameof(WarehouseManagementApiTestCollection))]
    public abstract class BaseIntegrationTest
    {
        protected readonly CustomWebApplicationFactory factory;
        protected readonly IWarehouseManagementContext context;
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;

        public BaseIntegrationTest(WarehouseManagementApiFixture fixture)
        {
            factory = fixture.Factory;
            context = fixture.Context;
            unitOfWork = fixture.UnitOfWork;

            Profile[] profiles = { new APIMappers(), new MapperService() };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(profiles);
            });

            mapper = config.CreateMapper();
        }
    }
}
