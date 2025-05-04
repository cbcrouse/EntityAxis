using AutoMapper;
using SampleApp.Application.Models;
using SampleApp.Domain;
using SampleApp.Persistence.Entities;

namespace SampleApp.Infrastructure.Mapping;

public class EntityProfile : Profile
{
    public EntityProfile()
    {
        // Product mappings (int to int)
        CreateMap<ProductCreateModel, Product>();
        CreateMap<ProductUpdateModel, Product>();
        CreateMap<Product, ProductDbEntity>().ReverseMap();

        // Order mappings (string to Guid)
        CreateMap<OrderCreateModel, Order>();
        CreateMap<OrderUpdateModel, Order>();
        CreateMap<Order, OrderDbEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.Empty : Guid.Parse(src.Id)))
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
    }
}
