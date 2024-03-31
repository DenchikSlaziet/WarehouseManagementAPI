using AutoMapper;
using WarehouseManagement.Services.Contracts.Models;
using WarehouseManagement.Services.Contracts.ModelsRequest;
using WarehouseManagement.API.Models.CreateRequest;
using WarehouseManagement.API.Models.Request;
using WarehouseManagement.API.Models.Response;

namespace WarehouseManagement.API.Mappers
{
    public class APIMappers : Profile
    {
        public APIMappers()
        {
            CreateMap<ProductModel, ProductResponse>(MemberList.Destination).ReverseMap();
            CreateMap<WarehouseUnitModel, WarehouseUnitResponse>(MemberList.Destination).ReverseMap();
            CreateMap<WarehouseModel, WarehouseResponse>(MemberList.Destination).ReverseMap();

            CreateMap<ProductCreateRequest, ProductModel>(MemberList.Destination)
                .ForMember(x => x.Id, opt => opt.Ignore()).ReverseMap();
            CreateMap<WarehouseCreateRequest, WarehouseModelRequest>(MemberList.Destination)
                .ForMember(x => x.Id, opt => opt.Ignore()).ReverseMap();
            CreateMap<WarehouseUnitCreateRequest, WarehouseUnitModelRequest>(MemberList.Destination)
                .ForMember(x => x.Id, opt => opt.Ignore()).ReverseMap();

            CreateMap<ProductRequest, ProductModel>(MemberList.Destination).ReverseMap();
            CreateMap<WarehouseRequest, WarehouseModelRequest>(MemberList.Destination).ReverseMap();
            CreateMap<WarehouseUnitRequest, WarehouseUnitModelRequest>(MemberList.Destination).ReverseMap();
        }
    }
}
