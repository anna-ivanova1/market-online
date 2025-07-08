using AutoMapper;

using CatalogService.API.Models;
using CatalogService.Domain.Entities;

namespace CatalogService.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Money, MoneyModel>().ReverseMap();
        CreateMap<Category, CategoryModel>()
            .ForMember(dest => dest.ParentId, rule => rule.MapFrom(src => src.Parent != null ? src.Parent.Id : (Guid?)null));

        CreateMap<CategoryModel, Category>();

        CreateMap<Product, ProductModel>().ReverseMap();
    }
}
