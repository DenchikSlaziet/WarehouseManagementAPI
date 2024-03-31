using AutoMapper;
using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Services.Contracts.Models;
using WarehouseManagement.Services.Contracts.ModelsRequest;

namespace WarehouseManagement.Services.Mappers
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            CreateMap<Product, ProductModel>(MemberList.Destination).ReverseMap();

            CreateMap<Warehouse, WarehouseModel>(MemberList.Destination)
                .ForMember(x => x.WarehouseUnitModels, opt => opt.Ignore()).ReverseMap();

            CreateMap<WarehouseUnit, WarehouseUnitModel>(MemberList.Destination)
                .ForMember(x => x.Product, opt => opt.Ignore()).ReverseMap();
          
            CreateMap<WarehouseModelRequest, Warehouse>(MemberList.Destination)
                .ForMember(x => x.WarehouseWarehouseUnits, opt => opt.Ignore())
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.DeletedAt, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedBy, opt => opt.Ignore()).ReverseMap();

            CreateMap<WarehouseUnitModelRequest, WarehouseUnit>(MemberList.Destination)
                .ForMember(x => x.Product, opt => opt.Ignore())
                .ForMember(x => x.WarehouseWarehouseUnits, opt => opt.Ignore())
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.DeletedAt, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedBy, opt => opt.Ignore()).ReverseMap();
        }
    }
}
