using AutoMapper;
using SampleApp.Application.Models;
using SampleApp.Domain;
using SampleApp.Persistence.Entities;

namespace SampleApp.Infrastructure.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductCreateModel, Product>();
        CreateMap<ProductUpdateModel, Product>();
        CreateMap<Product, ProductDbEntity>().ReverseMap();
    }
}